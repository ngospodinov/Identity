using System.Security.Claims;
using IdentityProjectUI.Models;
using IdentityProjectUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProjectUI.Controllers;

[Authorize]
public class AccessRequestsController(IdentityApiClient apiClient) : Controller
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct, int pageSize  = 10, int pageNumber = 1)
    {
        var accessRequests = await apiClient.GetAccessRequestsAsync(pageSize, pageNumber, ct);
        
        return View(accessRequests);
    }

    [HttpPost]
    public async Task<IActionResult> Accept(int id, [FromForm] List<int>? excludedItemIds, CancellationToken ct)
    {
        var decision = new AccessRequestDecision()
        {
            AccessRequestId = id,
            IsApproved = true,
            ExcludedItemIds = excludedItemIds ?? []
        };
        var success = await apiClient.AccessRequestDecisionAsync(decision, ct);
        if (!success)
        {
            ModelState.AddModelError("", "Approving access request failed. Please try again.");
        }
        
        return RedirectToAction("List", "AccessRequests");
    }


    [HttpPost]
    public async Task<IActionResult> Deny(int id, CancellationToken ct)
    {
        var decision = new AccessRequestDecision()
        {
            AccessRequestId = id,
            IsApproved = false,
        };
        var success = await apiClient.AccessRequestDecisionAsync(decision, ct);
        if (!success)
        {
            ModelState.AddModelError("", "Denying access request failed. Please try again.");
        }
        
        return RedirectToAction("List", "AccessRequests");
    }
    
    [HttpGet]
    public async Task<IActionResult> GetItems(
        [FromQuery] string category,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var currentUserId =
            User.FindFirst("sub")?.Value ??
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(currentUserId))
            return Unauthorized();

        var paged = await apiClient.GetDataItemsByCategoryAsync(pageSize, page, category, ct);
        var items = paged?.Items ?? [];

        return Json(items.Select(i => new { id = i.Id, title = i.Key, category = i.Category }));
    }
}