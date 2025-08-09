using IdentityProjectUI.Models;
using IdentityProjectUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProjectUI.Controllers;

[Authorize]
public class RequestAccessController(IdentityApiClient apiClient) : Controller
{
    [HttpGet]
    public async Task<IActionResult> List(int pageSize = 10, int pageNumber = 1, string? search = null, CancellationToken ct = default)
    {
        var users = await apiClient.GetUsersAsync(pageSize, pageNumber, search, ct);
        
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> NewRequest(string id,  int pageSize = 10, int pageNumber = 1, string? categoryFilter = null, CancellationToken ct = default)
    {
        var potentialItems = await apiClient.GetDataItemsByCategoryAsync(pageSize, pageNumber, categoryFilter, ct);
            
        return View(new NewAccessModel
        {
            TargetUserId = id,
            DataItems = potentialItems ?? new PagedResult<DataItemDto>()
        });
    }

    [HttpPost]
    public async Task<IActionResult> NewRequest(NewAccessModel model, CancellationToken ct)
    {
        var newAccessRequests = new NewAccessRequestsDto()
        {
            TargetUserId = model.TargetUserId,
            SelectedCategories = model.SelectedCategories,
        };
        
        var success = await apiClient.PostNewAccessRequestAsync(newAccessRequests, ct);
        if (!success)
        {
            ModelState.AddModelError("", "Deleting failed. Please try again.");
        }
        
        return RedirectToAction("List", model);
    }
}