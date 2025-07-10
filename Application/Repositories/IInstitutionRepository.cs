using Application.Handlers.Institutions.Dtos;
using Domain.Entities;

namespace Application.Repositories;

public interface IInstitutionRepository
{
    Task CreateInstitutionAsync(Institution institution, CancellationToken cancellationToken);
    
    Task<List<InstitutionDto>> GetInstitutionsAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);

    Task<Institution?> GetInstitutionByIdAsync(Guid userId, CancellationToken cancellationToken);

    Task<Guid?> GetInstitutionIdByClientAsync(string clientId, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(Guid institutionId, CancellationToken cancellationToken);

    Task DeleteInstitutionAsync(Guid institutionId, CancellationToken cancellationToken);
}