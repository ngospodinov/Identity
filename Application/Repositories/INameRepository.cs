using Application.Handlers.Users.Dtos;
using Domain.Entities;
using Domain.Enums;

namespace Application.Repositories;

public interface INameRepository
{
    Task<int> CountAsync(string? userId, CancellationToken cancellationToken);
    
    Task<List<Name>> GetByUserAsync(string userId, bool includeDeleted, CancellationToken ct);
    
    Task<List<Name>> GetForCategoryAsync(string userId, DataCategory category, bool includeDeleted, CancellationToken ct);
    
    Task<Name?> GetByIdAsync(int id, CancellationToken ct);

    Task<Name?> GetByIdAsync(int id, string userId, CancellationToken ct);

    Task<bool> ExistsAsync(string dataOwnerUserId, string firstName, DataCategory category, string? middleName, string? lastName, CancellationToken ct);
    
    Task CreateAsync(Name name, CancellationToken ct);
    
    Task UnsetDefaultsAsync(string userId, IEnumerable<DataCategory> categories, CancellationToken ct);
    
    Task SoftDeleteAsync(int id, CancellationToken ct);

    Task<bool> HasDefaultAsync(string userId, DataCategory category, CancellationToken ct);
    
    Task<bool> HasDefaultPublicProfileAsync(string userId, CancellationToken ct);

    Task SaveChangesAsync(CancellationToken ct);
}