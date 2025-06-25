using Application.Common;
using Application.Handlers.AccessGrants.Delete;
using Application.Handlers.AccessGrants.Get.GetPaged.User;
using Application.Handlers.AccessGrants.GetGrantedCategories;
using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/access-grants")]
public class AccessGrantController(ISender sender, ICurrentClientProvider currentClientProvider) : ControllerBase
{
    [HttpGet("me", Name = "GetMyAccessGrants")]
    public async Task<IActionResult> GetAccessGrantsAsync([FromQuery] string? category, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        var userId = User.FindFirst("sub")!.Value;
        
        return Ok(await sender.Send(new GetPagedAccessGrantsRequest(userId, category?.ParseDataCategory(), pageSize, pageNumber)));
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<IActionResult> GetGrantedCategoriesAsync([FromRoute] string userId, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("sub")!.Value;
        
        return Ok(await sender.Send(new GetGrantedCategoriesRequest(loggedUserId, userId), ct));
    }
    
    [HttpDelete("me/{accessGrantId:int}")]
    public async Task<IActionResult> RevokeAccessGrantAsync([FromRoute] int accessGrantId)
    {
        await sender.Send(new RevokeAccessGrantRequest(User.FindFirst("sub")!.Value, accessGrantId));
        
        return NoContent();
    }}