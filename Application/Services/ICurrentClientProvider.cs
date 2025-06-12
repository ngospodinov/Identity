namespace Application.Services;

public interface ICurrentClientProvider
{
    string? GetCurrentClientId();
}