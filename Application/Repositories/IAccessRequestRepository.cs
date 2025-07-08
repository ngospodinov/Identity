using Application.Handlers.Institutions.Dtos;
using Domain.Entities;

namespace Application.Repositories;

public interface IAccessRequestRepository
{
    Task CreateAsync(AccessRequest accessRequest, CancellationToken cancellationToken);
    
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);

    Task<AccessRequestDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
}