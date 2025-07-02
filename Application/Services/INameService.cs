using Application.Handlers.Users.Dtos;

namespace Application.Services;

public interface INameService
{
    Task<List<NameDto>> GetMyNamesAsync(string userId, CancellationToken ct);

    Task<List<DisplayNameDto>> RetrieveAllowedNamesAsync(
        string requesterId,
        string dataOwnerId,
        CancellationToken ct);
}