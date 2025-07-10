using Application.Handlers.Institutions.Dtos;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AccessRequestRepository(IdentityDbContext dbContext) : IAccessRequestRepository
{
    public async Task CreateAsync(AccessRequest accessRequest, CancellationToken cancellationToken)
    {
        await dbContext.AddAsync(accessRequest, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
    {
        return await dbContext.AccessRequests.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<AccessRequestDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await dbContext.AccessRequests
            .Where(x => x.Id == id)
            .Select(x => new AccessRequestDto
            {
                Id = x.Id,
                InstitutionId = x.InstitutionId,
                UserId = x.UserId,
                RequestedCategory = x.RequestedCategory,
                RequestedItemId = x.RequestedItemId,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AccessRequest?> GetEntityAsync(int id, CancellationToken cancellationToken)
    {
        return await dbContext.AccessRequests.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    
    public Task<List<AccessRequestDto>> GetAccessRequestsAsync(Guid userId, int pageNumber, int pageSize, Guid? institutionId, CancellationToken cancellationToken)
    {
        var query = dbContext.AccessRequests.Where(x => x.UserId == userId && (institutionId == null || x.InstitutionId == institutionId.Value));
        
        var accessRequests = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AccessRequestDto()
            {
                Id = x.Id,
                InstitutionId = x.InstitutionId, 
                RequestedCategory = x.RequestedCategory,
                RequestedItemId = x.RequestedItemId,
            })
            .ToListAsync(cancellationToken);
        
        return accessRequests;
    }
}