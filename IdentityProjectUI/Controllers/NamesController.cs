using IdentityProjectUI.Models;
using IdentityProjectUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProjectUI.Controllers
{
    [Authorize]
    public class NamesController(IdentityApiClient apiClient) : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(NameRowModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View();
            
            var nameDto = new NameDto
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Category = model.Category,
                IsDefaultForCategory = model.IsDefaultForCategory,
            };

            var ok = await apiClient.PostNameAsync(nameDto, ct);
            if (!ok)
            {
                ModelState.AddModelError("", "Saving failed. Please try again.");
                return View(model);
            }
            
            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<IActionResult> List(CancellationToken ct, int pageSize = 10, int pageNumber = 1)
        {
            var names = await apiClient.GetMyNamesAsync(pageSize, pageNumber, ct);
            
            return View(names);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var name = await apiClient.GetNameAsync(id, ct);
            
            if (name == null) return NotFound();
            
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NameDto model, CancellationToken ct)
        {
            var name = await apiClient.EditNameAsync(model, ct);

            return RedirectToAction("List", "Names");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var success =  await apiClient.DeleteNameAsync(id, ct);
            if (!success)
            {
                ModelState.AddModelError("", "Deleting failed. Please try again.");
            }
            
            return RedirectToAction("List", "Names");

        }
    }
}
