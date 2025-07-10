using Application.Handlers.Users.Dtos;
using Application.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AccessGrantRepository(IdentityDbContext dbContext) : IAccessGrantRepository
{
    public Task<List<AccessGrantDto>> GetAccessGrantsAsync(Guid userId, DataCategory? category, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = dbContext.AccessGrants.Where(x => x.UserId == userId && (category == null || x.Category == category));
        
        var accessGrants = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AccessGrantDto
            {
                Id = x.Id,
                InstitutionId = x.InstitutionId, 
                Category = x.Category,
                GrantedAt = x.GrantedAt,
            })
            .ToListAsync(cancellationToken);
        
        return accessGrants;
    }

    public async Task<AccessGrant?> GetAccessGrantByUserIdAsync(Guid userId, int accessGrantId, CancellationToken cancellationToken)
    {
        return await dbContext.AccessGrants.SingleOrDefaultAsync(x => x.Id == accessGrantId && x.UserId == userId, cancellationToken);
    }

    public async Task CreateAccessGrantAsync(AccessGrant accessGrant, CancellationToken cancellationToken)
    {
        await dbContext.AccessGrants.AddAsync(accessGrant, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid institutionId, DataCategory? category, int? requestedItemId,
        CancellationToken cancellationToken)
    {
        if (category.HasValue)
        {
            return await dbContext.AccessGrants.AnyAsync(x =>
                    x.UserId == userId &&
                    x.InstitutionId == institutionId &&
                    x.Category == category.Value &&
                    x.RevokedAt == null,
                cancellationToken);
        }

        if (requestedItemId.HasValue)
        {
            return await dbContext.AccessGrants.AnyAsync(x =>
                    x.UserId == userId &&
                    x.InstitutionId == institutionId &&
                    x.RequestedItemId == requestedItemId.Value &&
                    x.RevokedAt == null,
                cancellationToken);
        }

        return false;
    }
}