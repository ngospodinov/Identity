using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace IdentityProjectUI.Services;

public class AccessTokenHandler(IHttpContextAccessor accessor, ILogger<AccessTokenHandler> log) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var http = accessor.HttpContext;
        string? token = null;

        if (http?.User?.Identity?.IsAuthenticated == true)
        {
            token = await http.GetTokenAsync("access_token");
            if (string.IsNullOrEmpty(token))
                token = (await http.AuthenticateAsync("Cookies")).Properties?.GetTokenValue("access_token");
        }

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            log.LogDebug("Attached bearer token ({Len} chars)", token.Length);
        }
        else
        {
            log.LogWarning("No access token found to attach.");
        }

        return await base.SendAsync(request, ct);
    }
}