using Application.Handlers.Users.Dtos;

namespace Application.Services;

public interface IUserSyncService
{
    Task SyncUserCreatedAsync(UserCreatedDto dto, CancellationToken ct);
    
    Task SyncUserDeletedAsync(UserDeletedDto dto, CancellationToken ct);
}