using System.IdentityModel.Tokens.Jwt;
using Application.Repositories;
using Application.Services;
using Domain.Entities;

namespace Infrastructure.Services;

public class CurrentClientProvider(IHttpContextAccessor contextAccessor) : ICurrentClientProvider
{
    public string GetLoggedUserId() => contextAccessor.HttpContext!.User.FindFirst("sub")!.Value;

    public string? GetCurrentClientId()
    {
        return contextAccessor.HttpContext?.User
            .Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
    }

    
    public bool HasScope(string requiredScope)
    {
        var token = contextAccessor.HttpContext?.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token))
        {
            return false;  
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var scopeClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "scope" && c.Value == requiredScope);
            
            return scopeClaim != null;
        }
        catch (Exception)
        {
            return false; 
        }
    }
}