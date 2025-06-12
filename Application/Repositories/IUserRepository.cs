using Domain.Entities;

namespace Application.Repositories;

public interface IUserRepository
{
    Task CreateUserAsync(UserEntity user, CancellationToken cancellationToken);
    
    Task<UserEntity?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
}