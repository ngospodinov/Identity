using System.Net;
using Application.Handlers.AccessGrants.Get.GetPaged;
using Application.Handlers.Users.Create;
using Application.Handlers.Users.DataItems.Create;
using Application.Handlers.Users.DataItems.Delete;
using Application.Handlers.Users.DataItems.Get.GetPaged;
using Application.Handlers.Users.DataItems.Update;
using Application.Handlers.Users.Get;
using Application.Handlers.Users.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController(ISender sender) : ControllerBase
{
    [HttpGet("{userId:guid}", Name="GetUserById")]
    public async Task<IActionResult> GetUserAsync([FromRoute] Guid userId)
    {
        var result = await sender.Send(new GetUserRequest(userId));

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        var userId = await sender.Send(request);

        return StatusCode((int)HttpStatusCode.Created, userId);
    }

    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> UpdateUserAsync([FromRoute] Guid userId, [FromBody] UpdateUserRequest request)
    {
        request.SetId(userId);
        await sender.Send(request);
        
        return NoContent();
    }

    [HttpGet("{userId:guid}/data-items")]
    public async Task<IActionResult> GetDataItemsAsync([FromRoute] Guid userId, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        return Ok(await sender.Send(new GetPagedDataItemsRequest(userId, pageSize, pageNumber)));
    }

    [HttpPost("{userId:guid}/data-items")]
    public async Task<IActionResult> CreateDataItemAsync([FromRoute] Guid userId, [FromBody] CreateDataItemRequest request)
    {
        request.SetId(userId);
        var newDataItemId = await sender.Send(request);
        
        return CreatedAtRoute("GetUserById", new { userId = userId }, new { dataItemId = newDataItemId });
    }
    
    [HttpDelete("{userId:guid}/data-items/{dataItemId:int}")]
    public async Task<IActionResult> DeleteDataItemAsync([FromRoute] Guid userId, [FromRoute] int dataItemId)
    {
        await sender.Send(new DeleteDataItemRequest(userId, dataItemId));
        
        return NoContent();
    }
    
    [HttpPut("{userId:guid}/data-items/{dataItemId:int}")]
    public async Task<IActionResult> UpdateDataItemAsync([FromRoute] Guid userId, [FromRoute] int dataItemId, [FromBody] UpdateDataItemRequest request)
    {
        request.SetId(userId, dataItemId);
        await sender.Send(request);
        
        return NoContent();
    }
    
    [HttpGet("{userId:guid}/access-grants")]
    public async Task<IActionResult> GetAccessGrantsAsync([FromRoute] Guid userId, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        return Ok(await sender.Send(new GetPagedAccessGrantsRequest(userId, pageSize, pageNumber)));
    }
}