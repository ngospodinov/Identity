using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProjectUI.Controllers;

public class AuthController : Controller
{
    [HttpGet("/login")]
    public IActionResult Login(string? returnUrl = "/") =>
        Challenge(new AuthenticationProperties { RedirectUri = returnUrl ?? "/" },
            OpenIdConnectDefaults.AuthenticationScheme);

    [Authorize]
    [HttpGet("/logout")]
    public IActionResult Logout() =>
        SignOut(new AuthenticationProperties { RedirectUri = "/" },
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
}