using Application.Handlers.Users.Dtos;
using Domain.Enums;

namespace Application.Repositories;

public interface IDataItemRepository
{
    Task<List<UserDataItemDto>> GetUserDataItemsAsync(string userId, DataCategory? category, int pageNumber, int pageSize,
        CancellationToken cancellationToken);
    
    Task<List<UserDataItemDto>> GetUserDataItemsForCategoryAsync(string userId, DataCategory category, CancellationToken cancellationToken); 
    
    Task<UserDataItemDto?> GetDataItemByIdAsync(string userId, int dataItemId, CancellationToken cancellationToken);
    
    Task<bool> ExistsAsync(int dataItemId, CancellationToken cancellationToken);
    
    Task<int> CountAsync(string? userId, CancellationToken cancellationToken);
    
    Task<DataCategory?> GetCategoryAsync(int dataItemId, CancellationToken cancellationToken);
}