using System.Text.Json;
using IdentityProvider.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace IdentityProvider.Services;

public sealed class OutboxInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData ev,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        var ctx = ev.Context;
        if (ctx is null) return ValueTask.FromResult(result);

        var now = DateTime.UtcNow;

        var addedUsers = ctx.ChangeTracker.Entries<ApplicationUser>()
            .Where(e => e.State == EntityState.Added)
            .Select(e => new { e.Entity.Id, e.Entity.UserName, e.Entity.Email })
            .ToList();

        var deletedUsers = ctx.ChangeTracker.Entries<ApplicationUser>()
            .Where(e => e.State == EntityState.Deleted)
            .Select(e => new { e.Entity.Id })
            .ToList();

        if (addedUsers.Count == 0 && deletedUsers.Count == 0)
            return ValueTask.FromResult(result);

        var msgs = new List<OutboxMessage>(addedUsers.Count + deletedUsers.Count);

        foreach (var u in addedUsers)
        {
            msgs.Add(new OutboxMessage
            {
                Id = Guid.NewGuid(),
                CreatedOnUtc = now,
                Type = "idp.user.created",
                Payload = JsonSerializer.Serialize(new { u.Id, u.UserName, u.Email })
            });
        }

        foreach (var u in deletedUsers)
        {
            msgs.Add(new OutboxMessage
            {
                Id = Guid.NewGuid(),
                CreatedOnUtc = now,
                Type = "idp.user.deleted",
                Payload = JsonSerializer.Serialize(new { u.Id })
            });
        }

        ctx.Set<OutboxMessage>().AddRange(msgs);

        return ValueTask.FromResult(result);
    }
}