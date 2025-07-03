using Application.Handlers.Users.Dtos;
using Application.Repositories;
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
                ClientId = x.ClientId, 
                Category = x.Category,
                GrantedAt = x.GrantedAt,
            })
            .ToListAsync(cancellationToken);
        
        return accessGrants;
    }
}