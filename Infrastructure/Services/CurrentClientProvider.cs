using Application.Services;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class CurrentClientProvider(IHttpContextAccessor contextAccessor) : ICurrentClientProvider
{
    public string? GetCurrentClientId()
    {
        return contextAccessor.HttpContext?.User
            .Claims.FirstOrDefault(c => c.Type == "client_id")?.Value;
    }
}