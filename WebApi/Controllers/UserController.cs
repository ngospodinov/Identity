using Application.Handlers.AccessGrants.Create;
using Application.Handlers.AccessGrants.Delete;
using Application.Handlers.AccessGrants.Get.GetPaged.User;
using Application.Handlers.Names.GetDisplayNames;
using Application.Handlers.Users.Get;
using Application.Handlers.Users.GetPaged;
using Application.Handlers.Users.GetPagedGranted;
using Application.Handlers.Users.Update;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]

public class UserController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsersAsync([FromQuery] int pageSize, [FromQuery] int pageNumber,
        [FromQuery] string? search, CancellationToken ct)
    {
        var currentUserId = User.FindFirst("sub")!.Value;
        var users = await sender.Send(new GetPagedUsersRequest(currentUserId, pageSize, pageNumber, search), ct);
        
        return Ok(users);
    }
    
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserAsync([FromRoute] string userId, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("sub")!.Value;
        var request = new GetUserRequest(userId, loggedUserId);
        
        return Ok(await sender.Send(request, ct));
    }
    
    [HttpGet("granted")]
    public async Task<IActionResult> GetGrantedUsersAsync([FromQuery] int pageSize, [FromQuery] int pageNumber,
        [FromQuery] string? search, CancellationToken ct)
    {
        var currentUserId = User.FindFirst("sub")!.Value;
        var users = await sender.Send(new GetPagedGrantedUsersRequest(currentUserId, pageSize, pageNumber, search), ct);
        
        return Ok(users);
    }
    
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUserAsync([FromRoute] string userId, [FromBody] UpdateUserRequest request)
    {
        request.SetId(userId);
        await sender.Send(request);
        
        return NoContent();
    }

    
    [HttpGet("{userId}/access-grants", Name = "GetUserAccessGrants")]
    public async Task<IActionResult> GetAccessGrantsAsync([FromRoute] string userId, [FromQuery] DataCategory? category, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        return Ok(await sender.Send(new GetPagedAccessGrantsRequest(userId, category, pageSize, pageNumber)));
    }

    [HttpPost("{userId}/access-grants")]
    public async Task<IActionResult> CreateAccessGrant([FromRoute] string userId,
        [FromBody] CreateAccessGrantRequest request)
    {
        request.SetId(userId);
        var accessGrantId = await sender.Send(request);
        
        return CreatedAtRoute("GetUserAccessGrants", new { userId }, new { accessGrantId });
    }

    [HttpDelete("{userId}/access-grants/{accessGrantId:int}")]
    public async Task<IActionResult> RevokeAccessGrant([FromRoute] string userId, [FromRoute] int accessGrantId)
    {
        await sender.Send(new RevokeAccessGrantRequest(userId, accessGrantId));
        return NoContent();
    }

   
    
    [HttpGet("{dataOwnerId}/display-names")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetDisplayNames(
        string dataOwnerId,
        [FromQuery] DataCategory[]? categories,
        CancellationToken ct)
    {
        var requesterId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(requesterId))
            return Unauthorized();

        var isAdmin = User.IsInRole("Admin");

        IReadOnlyCollection<DataCategory>? filter = categories?
            .Distinct()
            .ToArray();

        var dto = await sender.Send(
            new GetDisplayNamesRequest(dataOwnerId, requesterId, filter, isAdmin),
            ct);
        return Ok(dto);
    }
}