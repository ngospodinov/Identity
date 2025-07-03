using Application.Handlers.Users.Dtos;
using Domain.Enums;

namespace Application.Repositories;

public interface IAccessGrantRepository
{
    Task<List<AccessGrantDto>> GetAccessGrantsAsync(Guid userId, DataCategory? category, int pageNumber, int pageSize, CancellationToken cancellationToken);
}