using Duende.IdentityServer.Stores;
using IdentityProvider.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityProvider.Services;

public class DeletionService(
    UserManager<ApplicationUser> users,
    IPersistedGrantStore grants)
{
    public async Task HardDeleteAsync(string userId, CancellationToken ct)
    {
        var user = await users.FindByIdAsync(userId) ?? throw new KeyNotFoundException($"User with id {userId} was not found.");

        await grants.RemoveAllAsync(new PersistedGrantFilter
        {
            SubjectId = user.Id
        });
        
        var result = await users.DeleteAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
    }
}