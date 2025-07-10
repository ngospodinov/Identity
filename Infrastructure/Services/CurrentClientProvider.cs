using Application.Repositories;
using Application.Services;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class CurrentClientProvider(IHttpContextAccessor contextAccessor, IInstitutionRepository institutionRepository) : ICurrentClientProvider
{
    public string? GetCurrentClientId()
    {
        return contextAccessor.HttpContext?.User
            .Claims.FirstOrDefault(c => c.Type == "client_id")?.Value;
    }

    public async Task<Guid?> MapClientToInstitutionId(string clientId, CancellationToken cancellationToken)
    {
        return await institutionRepository.GetInstitutionIdByClientAsync(clientId, cancellationToken);
    }
}