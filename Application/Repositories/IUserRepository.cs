using Domain.Entities;

namespace Application.Repositories;

public interface IUserRepository
{
    Task CreateUserAsync(User user, CancellationToken cancellationToken);
    
    Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    
    Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken);
}