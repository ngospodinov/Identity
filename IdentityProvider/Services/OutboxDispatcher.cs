using System.Text;
using IdentityProvider.Data;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace IdentityProvider.Services;

public sealed class OutboxDispatcher(IServiceProvider sp, IConfiguration configuration) : BackgroundService
{
    private readonly string _host = configuration["RabbitMq:Host"] ?? "localhost";
    private readonly int _port = int.TryParse(configuration["RabbitMq:Port"], out int port) ? port : 5672;
    private readonly string _user = configuration["RabbitMq:User"] ?? "guest";
    private readonly string _pass = configuration["RabbitMq:Pass"] ?? "guest";
    private readonly string _queue = configuration["RabbitMq:Queue"] ?? "idp.users";
    private readonly string _vhost = configuration["RabbitMq:VirtualHost"]?? "/";

    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName   = _host,
            Port       = _port,
            UserName   = _user,
            Password   = _pass,
            VirtualHost= _vhost
        };
        
        await using var connection = await factory.CreateConnectionAsync(cancellationToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        const string queue = "idp.users";
        await channel.QueueDeclareAsync(queue: queue, durable: true, autoDelete: false, exclusive: false, arguments: null, cancellationToken: cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
        {
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var messages = await db.Outbox
                .Where(x => x.ProcessedOnUtc == null)
                .OrderBy(x => x.CreatedOnUtc)
                .Take(20)
                .ToListAsync(cancellationToken);

            if (messages.Count == 0)
            {
                await Task.Delay(750, cancellationToken);
                continue;
            }

            foreach (var m in messages)
            {
                var body = Encoding.UTF8.GetBytes(m.Payload);

                var props = new BasicProperties
                {
                    ContentType = "application/json",
                    DeliveryMode = DeliveryModes.Persistent, 
                    Type = m.Type,
                    MessageId = m.Id.ToString()
                };


                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: queue,
                    mandatory: false,
                    basicProperties: props,
                    body: body,
                    cancellationToken: cancellationToken);

                m.ProcessedOnUtc = DateTime.UtcNow;
                m.Error = null;
            }

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}

    