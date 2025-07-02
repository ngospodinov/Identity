using Application.Handlers.Users.Dtos;
using Application.Services;
using Infrastructure.Messaging;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserSyncService(ApplicationDbContext dbContext, ILogger<UserSyncService> log) : IUserSyncService
{
    public async Task SyncUserCreatedAsync(UserCreatedDto dto, CancellationToken ct)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == dto.Id, ct);
        if (user is null)
        {
            dbContext.Users.Add(new Domain.Entities.User
            {
                Id = dto.Id,
                UserName = dto.UserName ?? string.Empty,
                Email = dto.Email ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            });
        }
        else
        {
            user.UserName = dto.UserName ?? user.UserName;
            user.Email    = dto.Email    ?? user.Email;
        }
        await dbContext.SaveChangesAsync(ct);
        log.LogInformation("Synced user.created {Id}", dto.Id);
    }

    public async Task SyncUserDeletedAsync(UserDeletedDto dto, CancellationToken ct)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == dto.Id, ct);
        if (user is null) return;
        
        var grants = await dbContext.AccessGrants
            .Where(g => (g.DataOwnerUserId == user.Id 
                         || g.RequesterUserId == user.Id) && 
                            g.RevokedAt == null)                   
            .ToListAsync(ct);
        
        foreach (var g in grants)
        {
           g.RevokedAt = DateTime.UtcNow;
        }
        
        var items = await dbContext.UserDataItems
            .Where(x => x.UserId == user.Id && !x.IsDeleted)
            .ToListAsync(ct);
        
        foreach (var it in items)
        {
            it.IsDeleted = true; 
            it.DeletedAt = DateTime.UtcNow;
        }
        
        if (!user.IsDeleted)
        {
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(ct);
        log.LogInformation("Soft-deleted user and related data {Id}", user.Id);
    }
}