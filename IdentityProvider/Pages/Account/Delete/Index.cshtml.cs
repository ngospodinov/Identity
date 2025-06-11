using System.Globalization;
using Duende.IdentityServer.Services;
using IdentityProvider.Models;
using IdentityProvider.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityProvider.Pages.Account.Delete;

[Authorize]
public class IndexModel : PageModel
{
    private const string ReauthCookieName = "reauth_at";
    private static readonly TimeSpan ReauthWindow = TimeSpan.FromMinutes(5);

    private readonly DeletionService _deletions;
    private readonly UserManager<ApplicationUser> _users;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;


    public IndexModel(DeletionService deletions, UserManager<ApplicationUser> users,
        SignInManager<ApplicationUser> signIn, IIdentityServerInteractionService interaction)
    {
        _deletions = deletions;
        _users = users;
        _signInManager = signIn;
        _interaction = interaction;
    }

    public void OnGet()
    {
        ViewData["IsReauthenticated"] = IsReauthRecent();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!IsReauthRecent())
        {
            ModelState.AddModelError(string.Empty, "Please re-authenticate first.");
            ViewData["IsReauthenticated"] = false;
            return Page();
        }

        if (!Request.HasFormContentType || Request.Form["Confirm"] != "true")
        {
            ModelState.AddModelError(string.Empty, "Please confirm deletion.");
            ViewData["IsReauthenticated"] = true;
            return Page();
        }

        var user = await _users.GetUserAsync(User);
        if (user is null) return NotFound();

        await _deletions.HardDeleteAsync(user.Id, ct);

        var schemeProvider = HttpContext.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();
        var allSchemes = await schemeProvider.GetAllSchemesAsync();

        foreach (var s in allSchemes.Where(s => s.HandlerType?.Name?.Contains("CookieAuthenticationHandler") == true))
        {
            await HttpContext.SignOutAsync(s.Name);
        }

        await _signInManager.SignOutAsync(); 
        await HttpContext.SignOutAsync(Duende.IdentityServer.IdentityServerConstants.DefaultCookieAuthenticationScheme);   
        await HttpContext.SignOutAsync(Duende.IdentityServer.IdentityServerConstants.ExternalCookieAuthenticationScheme); 
   
        var logoutId = await _interaction.CreateLogoutContextAsync();
        await _interaction.RevokeTokensForCurrentSessionAsync(); 
        
        return RedirectToPage("/Account/Logout/LoggedOut", new { logoutId });
    }

    public IActionResult OnGetCallback()
    {
        var now = DateTime.UtcNow;

        Response.Cookies.Append(
            ReauthCookieName,
            now.ToString("O", CultureInfo.InvariantCulture),
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = now.Add(ReauthWindow)
            });
        

        return RedirectToPage("/Account/Delete/Index");
    }

    private bool IsReauthRecent()
    {
        if (!Request.Cookies.TryGetValue(ReauthCookieName, out var val)) return false;
        if (!DateTime.TryParse(val, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var when))
            return false;

        return DateTime.UtcNow - when <= ReauthWindow;
    }
}
