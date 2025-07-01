using Application.Handlers.Users.Dtos;
using Domain.Enums;

namespace Application.Repositories;

public interface IDataItemRepository
{
    Task<List<UserDataItemDto>> GetUserDataItemsAsync(Guid userId, DataCategory? category, int pageNumber, int pageSize,
        CancellationToken cancellationToken);
}