using Application.Handlers.Requesters.Dtos;
using Domain.Entities;
using Domain.Enums;

namespace Application.Repositories;

public interface IAccessRequestRepository
{
    Task<int> CountRequesterAsync(string? userId, CancellationToken cancellationToken);
    
    Task<int> CountOwnerAsync(string? userId, CancellationToken cancellationToken);

    
    Task CreateAsync(AccessRequest accessRequest, CancellationToken cancellationToken);
    
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);

    Task<AccessRequestDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<AccessRequest?> GetEntityAsync(int id, CancellationToken cancellationToken);

    Task<List<AccessRequestDto>> GetAccessRequestsAsync(string dataOwnerUserId, int pageNumber, int pageSize,
        CancellationToken cancellationToken);
    
    Task<List<AccessRequestDto>> GetAccessRequestsAsync(string dataOwnerUserId, int pageNumber, int pageSize, string? requesterUserId,
        CancellationToken cancellationToken);
    
    Task<List<AccessRequestDto>> GetAccessRequestsAsync(string requesterUserId, string? dataOwnerUserId, DataCategory? category,
        int pageNumber, int pageSize, CancellationToken cancellationToken);
}