using Application.Handlers.Users.Dtos;
using Domain.Entities;

namespace Application.Repositories;

public interface IUserRepository
{
    Task<int> CountAsync(CancellationToken cancellationToken);
    
    Task<List<UserLeanDto>> GetUsersAsync(string currentUserId, int pageNumber, int pageSize, string? search, CancellationToken ct);
    
    Task<List<UserLeanDto>> GetGrantedUsersAsync(string currentUserId, int pageNumber, int pageSize, string? search, CancellationToken ct);
    
    Task CreateUserAsync(User user, CancellationToken cancellationToken);
    
    Task<User?> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
    
    Task<bool> ExistsAsync(string userId, CancellationToken cancellationToken);
}