namespace Application.Services;

public interface ICurrentClientProvider
{
    string? GetCurrentClientId();

    bool HasScope(string requiredScope);
}