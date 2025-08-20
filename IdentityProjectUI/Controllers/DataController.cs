using IdentityProjectUI.Models;
using IdentityProjectUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProjectUI.Controllers;

[Authorize]
public class DataController(IdentityApiClient apiClient) : Controller
{
    [HttpGet]
    public async Task<IActionResult> List(int pageSize = 10, int pageNumber = 1, string? search = null, CancellationToken ct = default)
    {
        var users = await apiClient.GetGrantedUsersAsync(pageSize, pageNumber, search, ct);
        
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> ViewAvailable([FromQuery] string userId, [FromQuery] string category, int pageSize = 10, int pageNumber = 1, CancellationToken ct = default)
    {
        var user = await apiClient.GetUserAsync(userId, ct);
        var dataItems = await apiClient.GetDataItemsForCategoryByUserAsync(userId, pageSize, pageNumber, category, ct);
        var name = await apiClient.GetNameAsync(userId, category, ct);
        var model = new AvailableDataModel
        {
            UserData = user!,
            NameData = name!,
            DataItems = dataItems,
            Category = category,
        };
        
        return View(model);
    }
}