using Application.Handlers.Requesters.Dtos;
using Application.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AccessRequestRepository(ApplicationDbContext dbContext) : IAccessRequestRepository
{
    public async Task<int> CountRequesterAsync(string? userId, CancellationToken cancellationToken)
    {
        return await dbContext.AccessRequests
            .CountAsync(x => userId == null || x.RequesterUserId == userId, cancellationToken);
    }
    
    public async Task<int> CountOwnerAsync(string? userId, CancellationToken cancellationToken)
    {
        return await dbContext.AccessRequests
            .CountAsync(x => userId == null || x.DataOwnerUserId == userId, cancellationToken);    
    }
    
    public async Task CreateAsync(AccessRequest accessRequest, CancellationToken cancellationToken)
    {
        await dbContext.AddAsync(accessRequest, cancellationToken);
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
                RequesterUserId = x.RequesterUserId,
                DataOwnerUserId = x.DataOwnerUserId,
                RequestedCategory = x.RequestedCategory!.Value.ParseCategory(),
                RequestedItemId = x.RequestedItemId,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AccessRequest?> GetEntityAsync(int id, CancellationToken cancellationToken)
    {
        return await dbContext.AccessRequests.FirstOrDefaultAsync(x => x.Id == id, cancellationToken); 
    }
    
    public async Task<List<AccessRequestDto>> GetAccessRequestsAsync(string dataOwnerUserId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        
        var accessRequests = await dbContext.AccessRequests
            .Where(x => x.DataOwnerUserId == dataOwnerUserId)
            .OrderByDescending(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AccessRequestDto()
            {
                Id = x.Id,
                RequesterUserId = x.RequesterUserId, 
                RequesterEmail = x.RequesterUser.Email,
                RequesterUserName = x.RequesterUser.UserName,
                RequestedCategory = x.RequestedCategory == null ? null : x.RequestedCategory.Value.ParseCategory(),
                RequestedItemId = x.RequestedItemId,
                Status = x.Status.ParseStatus(),
            })
            .ToListAsync(cancellationToken);
        
        return accessRequests;
    }
    
    public async Task<List<AccessRequestDto>> GetAccessRequestsAsync(string dataOwnerUserId, int pageNumber, int pageSize, string? requesterUserId, CancellationToken cancellationToken)
    {
        var query = dbContext.AccessRequests.Where(x => x.DataOwnerUserId == dataOwnerUserId && (string.IsNullOrEmpty(requesterUserId) || x.RequesterUserId == requesterUserId));
        
        var accessRequests = await query
            .OrderByDescending(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AccessRequestDto()
            {
                Id = x.Id,
                RequesterUserId = x.RequesterUserId, 
                RequesterEmail = x.RequesterUser.Email,
                RequesterUserName = x.RequesterUser.UserName,
                RequestedCategory = x.RequestedCategory == null ? null : x.RequestedCategory.Value.ParseCategory(),
                RequestedItemId = x.RequestedItemId,
            })
            .ToListAsync(cancellationToken);
        
        return accessRequests;
    }
    
    public Task<List<AccessRequestDto>> GetAccessRequestsAsync(string requesterUserId, string? userId, DataCategory? category,
        int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = dbContext.AccessRequests.Where( x => 
            x.RequesterUserId == requesterUserId && (userId == null || x.DataOwnerUserId == userId)
                                             && (category == null || x.RequestedCategory == category));

        var accessRequests = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AccessRequestDto()
            {
                Id = x.Id,
                DataOwnerUserId = x.DataOwnerUserId,
                RequesterUserId = x.RequesterUserId,
                RequestedCategory = x.RequestedCategory != null ? x.RequestedCategory.Value.ParseCategory() : null,
                RequestedItemId = x.RequestedItemId,
                RequestedAt = x.RequestedAt,
                Status = x.Status.ParseStatus(),
            })
            .ToListAsync(cancellationToken);

        return accessRequests;
    }
}