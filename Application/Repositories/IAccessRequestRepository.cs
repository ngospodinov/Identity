using Application.Handlers.Institutions.Dtos;
using Domain.Entities;

namespace Application.Repositories;

public interface IAccessRequestRepository
{
    Task CreateAsync(AccessRequest accessRequest, CancellationToken cancellationToken);
    
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);

    Task<AccessRequestDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<AccessRequest?> GetEntityAsync(int id, CancellationToken cancellationToken);
    
    Task<List<AccessRequestDto>> GetAccessRequestsAsync(Guid userId, int pageNumber, int pageSize, Guid? institutionId,
        CancellationToken cancellationToken);
}