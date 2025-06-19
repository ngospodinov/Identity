using Application.Handlers.Users.Dtos;
using Domain.Entities;
using Domain.Enums;

namespace Application.Repositories;

public interface IAccessGrantRepository
{
    Task<HashSet<DataCategory?>> GetAllowedCategoriesAsync(string requesterId, string dataOwnerUserId, CancellationToken cancellationToken);
    
    Task<List<AccessGrantDto>> GetAccessGrantsAsync(string dataOwnerUserId, DataCategory? category, int pageNumber, int pageSize,
        CancellationToken cancellationToken);

    Task<AccessGrant?> GetActiveAccessGrantForCategoryAsync(string dataOwnerUserId,
        string requesterUserId, DataCategory category, CancellationToken ct);
    
    Task<List<AccessGrantDto>> GetAccessGrantsAsync(string dataOwnerUserId, string requesterId,
        CancellationToken cancellationToken);

    Task<List<AccessGrantDto>> GetAccessGrantsAsync(string requesterUserId, string? dataOwnerUserId, DataCategory? category,
        int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<AccessGrant?> GetAccessGrantByUserIdAsync(string dataOwnerUserId, int accessGrantId, CancellationToken cancellationToken);

    Task CreateAccessGrantAsync(AccessGrant accessGrant, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(string dataOwnerUserId, string requesterUserId, DataCategory? category, int? requestedItemId,
        CancellationToken cancellationToken);
    
    Task<int> CountRequesterAsync(string? userId, CancellationToken cancellationToken);
    
    Task<int> CountOwnerAsync(string? userId, CancellationToken cancellationToken);

}