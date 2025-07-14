using System.Text;
using System.Text.Json;
using Application.Handlers.Users.Dtos;
using Application.Services;
using RabbitMQ.Client;

namespace Infrastructure.Messaging;

public sealed class UserEventsConsumer(IConfiguration cfg, IServiceProvider sp, ILogger<UserEventsConsumer> log)
    : BackgroundService
{
    private readonly string _host  = cfg["RabbitMq:HostName"] ?? "localhost";
    private readonly int    _port  = int.TryParse(cfg["RabbitMq:Port"], out var p) ? p : 5672;
    private readonly string _user  = cfg["RabbitMq:UserName"] ?? "guest";
    private readonly string _pass  = cfg["RabbitMq:Password"] ?? "guest";
    private readonly string _vhost = cfg["RabbitMq:VirtualHost"] ?? "/";
    private readonly string _queue = cfg["RabbitMq:Queue"] ?? "idp.users";

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var factory = new ConnectionFactory
        {
            HostName = _host, Port = _port, UserName = _user, Password = _pass, VirtualHost = _vhost
        };

        while (!ct.IsCancellationRequested)
        {
            try
            {
                await using var conn = await factory.CreateConnectionAsync(ct);
                await using var ch   = await conn.CreateChannelAsync(cancellationToken: ct);

                await ch.QueueDeclareAsync(_queue, durable: true, exclusive: false, autoDelete: false,
                                           arguments: null, cancellationToken: ct);

                while (!ct.IsCancellationRequested)
                {
                    var msg = await ch.BasicGetAsync(_queue, autoAck: false, cancellationToken: ct);
                    if (msg is null)
                    {
                        await Task.Delay(500, ct);
                        continue;
                    }

                    try
                    {
                        var body = Encoding.UTF8.GetString(msg.Body.ToArray());
                        var type = msg.BasicProperties?.Type ?? "";

                        using var scope = sp.CreateScope();
                        var handler = scope.ServiceProvider.GetRequiredService<IUserSyncService>();

                        switch (type.ToLowerInvariant())
                        {
                            case "idp.user.created":
                                var created = JsonSerializer.Deserialize<UserCreatedDto>(body);
                                if (created is null) throw new InvalidOperationException("Invalid payload");
                                await handler.SyncUserCreatedAsync(created, ct);
                                break;

                            case "idp.user.deleted":
                                var deleted = JsonSerializer.Deserialize<UserDeletedDto>(body);
                                if (deleted is null) throw new InvalidOperationException("Invalid payload");
                                await handler.SyncUserDeletedAsync(deleted, ct);
                                break;

                            default:
                                log.LogWarning("Unknown event type {Type}, acknowledging", type);
                                break;
                        }

                        await ch.BasicAckAsync(msg.DeliveryTag, multiple: false, cancellationToken: ct);
                    }
                    catch (Exception ex)
                    {
                        var redelivered = msg.Redelivered;
                        log.LogError(ex, "Failed to process message {Tag} (redelivered={Redelivered})", msg.DeliveryTag, redelivered);

                        if (redelivered)
                            await ch.BasicRejectAsync(msg.DeliveryTag, requeue: false, cancellationToken: ct);
                        else
                            await ch.BasicNackAsync(msg.DeliveryTag, multiple: false, requeue: true, cancellationToken: ct);

                        await Task.Delay(250, ct);
                    }
                }
            }
            catch (OperationCanceledException) {log.LogWarning("UserEventsConsumer cancelled");}
            catch (Exception ex)
            {
                log.LogWarning(ex, "UserEventsConsumer reconnecting in 2s…");
                await Task.Delay(2000, ct);
            }
        }
    }
}

