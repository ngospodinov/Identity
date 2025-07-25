using IdentityProjectUI.Models;
using IdentityProjectUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProjectUI.Controllers;

[Authorize]
public class MyDataController(IdentityApiClient apiClient) : Controller
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct, int pageSize = 10, int pageNumber = 1)
    {
        var names = await apiClient.GetMyDataAsync(pageSize, pageNumber, ct);
            
        return View(names);
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Add(DataItemDto model, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View();
            
        var dataItemDto = new DataItemDto
        {
            Key = model.Key,
            Value = model.Value,
            Category = model.Category,
        };
    
        var ok = await apiClient.PostDataItemAsync(dataItemDto, ct);
        if (!ok)
        {
            ModelState.AddModelError("", "Saving failed. Please try again.");
            return View(model);
        }
            
        return RedirectToAction(nameof(List));
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var dataItem = await apiClient.GetDataItemAsync(id, ct);
            
        if (dataItem == null) return NotFound();
            
        return View(dataItem);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(DataItemDto model, CancellationToken ct)
    {
        var isSuccessful = await apiClient.EditDataItemAsync(model, ct);
        if (!isSuccessful)
        {
            ModelState.AddModelError("", "Editing failed. Please try again.");
        }
    
        return RedirectToAction("List", "MyData");
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var success =  await apiClient.DeleteDataItemAsync(id, ct);
        if (!success)
        {
            ModelState.AddModelError("", "Deleting failed. Please try again.");
        }
            
        return RedirectToAction("List", "MyData");
    }
}