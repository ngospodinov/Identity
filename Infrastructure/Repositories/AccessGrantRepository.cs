using Application.Handlers.Users.Dtos;
using Application.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AccessGrantRepository(ApplicationDbContext dbContext) : IAccessGrantRepository
{
    public async Task<int> CountRequesterAsync(string? userId, CancellationToken cancellationToken)
    {
        return await dbContext.AccessGrants
            .CountAsync(x => userId == null || x.RequesterUserId == userId, cancellationToken);
    }
    
    public async Task<int> CountOwnerAsync(string? userId, CancellationToken cancellationToken)
    {
        return await dbContext.AccessGrants
            .CountAsync(x => userId == null || x.DataOwnerUserId == userId, cancellationToken);
    }
    
    public async Task<HashSet<DataCategory?>> GetAllowedCategoriesAsync(string requesterId, string dataOwnerId,
        CancellationToken cancellationToken)
    {
        return await dbContext.AccessGrants
        .Where(x => x.RequesterUserId == requesterId && x.DataOwnerUserId == dataOwnerId && x.RevokedAt == null)
        .Select(x=> x.Category)
        .ToHashSetAsync(cancellationToken);
    }

    public Task<List<AccessGrantDto>> GetAccessGrantsAsync(string dataOwnerUserId, DataCategory? category, int pageNumber,
        int pageSize, CancellationToken cancellationToken)
    {
        var query = dbContext.AccessGrants.Where(
            x => x.DataOwnerUserId == dataOwnerUserId && (category == null || x.Category == category));

        var accessGrants = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AccessGrantDto
            {
                Id = x.Id,
                RequesterUserId = x.RequesterUserId,
                Category = x.Category!.Value.ParseCategory(),
                GrantedAt = x.GrantedAt,
                RevokedAt = x.RevokedAt,
            })
            .ToListAsync(cancellationToken);

        return accessGrants;
    }

    public async Task<List<AccessGrantDto>> GetAccessGrantsAsync(string dataOwnerUserId, string requesterId,
        CancellationToken cancellationToken)
    {
        return await dbContext.AccessGrants.Where(x =>
                x.RequesterUserId == requesterId && x.DataOwnerUserId == dataOwnerUserId && x.RevokedAt == null)
            .Select(ag => new AccessGrantDto
            {
                Id = ag.Id,
                RequesterUserId = ag.RequesterUserId,
                Category = ag.Category!.Value.ParseCategory(),
                GrantedAt = ag.GrantedAt,
                RevokedAt = ag.RevokedAt,
            }).ToListAsync(cancellationToken);
    }

    public async Task<AccessGrant?> GetActiveAccessGrantForCategoryAsync(string dataOwnerUserId,
        string requesterUserId, DataCategory category, CancellationToken ct)
    {
        return await dbContext.AccessGrants
            .Include(x => x.SpecificRevocations)
            .FirstOrDefaultAsync(x =>
                x.RequesterUserId == requesterUserId &&
                x.DataOwnerUserId == dataOwnerUserId &&
                x.Category == category && 
                x.RevokedAt == null, 
            cancellationToken: ct);
    }

    public Task<List<AccessGrantDto>> GetAccessGrantsAsync(string requesterUserId, string? userId, DataCategory? category,
        int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = dbContext.AccessGrants.Where( x => 
            x.RequesterUserId == requesterUserId && (userId == null || x.DataOwnerUserId == userId)
            && (category == null || x.Category == category));

        var accessGrants = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AccessGrantDto
            {
                Id = x.Id,
                DataOwnerUserId = x.DataOwnerUserId,
                RequesterUserId = x.RequesterUserId,
                Category = x.Category!.Value.ParseCategory(),
                GrantedAt = x.GrantedAt,
                RevokedAt = x.RevokedAt,
            })
            .ToListAsync(cancellationToken);

        return accessGrants;
    }

    public async Task<AccessGrant?> GetAccessGrantByUserIdAsync(string userId, int accessGrantId,
        CancellationToken cancellationToken)
    {
        return await dbContext.AccessGrants.SingleOrDefaultAsync(x => x.Id == accessGrantId && x.DataOwnerUserId == userId,
            cancellationToken);
    }

    public async Task CreateAccessGrantAsync(AccessGrant accessGrant, CancellationToken cancellationToken)
    {
        await dbContext.AccessGrants.AddAsync(accessGrant, cancellationToken);
    }

    public async Task<bool> ExistsAsync(string dataOwnerUserId, string requesterUserId, DataCategory? category, int? requestedItemId,
        CancellationToken cancellationToken)
    {
        if (category.HasValue)
        {
            return await dbContext.AccessGrants.AnyAsync(x =>
                    x.DataOwnerUserId == dataOwnerUserId &&
                    x.RequesterUserId == requesterUserId &&
                    x.Category == category.Value &&
                    x.RevokedAt == null,
                cancellationToken);
        }

        if (requestedItemId.HasValue)
        {
            return await dbContext.AccessGrants.AnyAsync(x =>
                    x.DataOwnerUserId == dataOwnerUserId &&
                    x.RequesterUserId == requesterUserId &&
                    x.RequestedItemId == requestedItemId.Value &&
                    x.RevokedAt == null,
                cancellationToken);
        }

        return false;
    }
}