using IdentityProvider.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityProvider.Pages.Account.Manage;

[Authorize]
public class IndexModel(UserManager<ApplicationUser> users) : PageModel
{
    public string UserId   { get; private set; } = string.Empty;
    public string UserName { get; private set; } = string.Empty;
    public string Email    { get; private set; } = string.Empty;

    public async Task OnGet()
    {
        var user = await users.GetUserAsync(User);
        if (user is null) return;

        UserId   = user.Id;
        UserName = user.UserName ?? string.Empty;
        Email    = user.Email    ?? string.Empty;
    }
}