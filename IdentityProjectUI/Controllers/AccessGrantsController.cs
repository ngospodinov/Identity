using System.Security.Claims;
using IdentityProjectUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProjectUI.Controllers;

[Authorize]
public class AccessGrantsController(IdentityApiClient apiClient) : Controller
{
    [HttpGet]    
    public async Task<IActionResult> List(CancellationToken ct, int pageSize  = 10, int pageNumber = 1)
    {
        var accessGrants = await apiClient.GetAccessGrantsAsync(pageSize, pageNumber, ct);
        
        return View(accessGrants);
    }

    
    [HttpPost]
    public async Task<IActionResult> Revoke(int id, CancellationToken ct)
    {
        var success = await apiClient.RevokeAccessGrantAsync(id, ct);
        if (!success)
        {
            ModelState.AddModelError("revokeGrant", "Revoking access grant failed. Please try again.");
        }
        
        return RedirectToAction("List", "AccessGrants");
    }

    [HttpGet]
    public async Task<IActionResult> GetCategoriesById([FromQuery] string userId, CancellationToken ct = default)
    {
        var currentUserId =
            User.FindFirst("sub")?.Value ??
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrWhiteSpace(currentUserId))
            return Unauthorized();

        var items = await apiClient.GetGrantedCategoriesAsync(userId, ct) ?? [];

        return Json(items.Select(i => new { id = i.Id, categoryName = i.CategoryName, grantedAt = i.GrantedAt }));
    }
}