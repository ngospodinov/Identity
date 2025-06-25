using Application.Common;
using Application.Handlers.DataItems.Create;
using Application.Handlers.DataItems.Delete;
using Application.Handlers.DataItems.Get.GetById;
using Application.Handlers.DataItems.Get.GetPaged;
using Application.Handlers.DataItems.Update;
using Application.Handlers.Users.Data;
using Application.Handlers.Users.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DataController(ISender sender) : ControllerBase
{
    [HttpGet("me")]
    public async Task<PagedResult<UserDataItemDto>> GetMyData([FromQuery] string? categoryFilter,
        [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1, CancellationToken ct = default)
    {
        return await sender.Send(new GetPagedDataItemsRequest(User.FindFirst("sub")!.Value, categoryFilter, pageSize, pageNumber), ct);
    }
    
    [HttpGet("me/{id:int}")]
    public async Task<UserDataItemDto?> GetByIdAsync([FromRoute] int id, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")!.Value;

        return await sender.Send(new GetDataItemRequest(id, userId), ct);
    }
    
    [HttpPost("me")]
    public async Task<IActionResult> CreateDataItemAsync([FromBody] UserDataItemDto dataItem, CancellationToken ct)
    {
        var request = new CreateDataItemRequest
        {
            DataOwnerUserId = User.FindFirst("sub")!.Value,
            Key = dataItem.Key,
            Value = dataItem.Value,
            Category = dataItem.Category.ParseDataCategory(),
        };
        
        var newDataItemId = await sender.Send(request, ct);

        return NoContent();
    }
    
    [HttpDelete("me/{dataItemId:int}")]
    public async Task<IActionResult> DeleteDataItemAsync([FromRoute] int dataItemId)
    {
        await sender.Send(new DeleteDataItemRequest(User.FindFirst("sub")!.Value, dataItemId));
        
        return NoContent();
    }
    
    [HttpPut("me/{dataItemId:int}")]
    public async Task<IActionResult> UpdateDataItemAsync([FromRoute] int dataItemId, [FromBody] UserDataItemDto dataItemDto)
    {
        var request = new UpdateDataItemRequest
        {
            DataOwnerUserId = User.FindFirst("sub")!.Value,
            DataItemId = dataItemId,
            Value = dataItemDto.Value,
            Category = dataItemDto.Category.ParseDataCategory(),
        };
        
        await sender.Send(request);
        
        return NoContent();
    }
    
    [HttpGet("{userId}", Name="GetUserDataForCategory")]
    public async Task<IActionResult> GetUserDataAsync([FromRoute] string userId, [FromQuery] string categoryFilter, [FromQuery] int pageSize = 10, 
        [FromQuery] int pageNumber = 1, CancellationToken ct = default)
    {
        var currentUser = User.FindFirst("sub")!.Value;
        var result = await sender.Send(new GetUserDataRequest(userId, currentUser, categoryFilter, pageSize, pageNumber), ct);

        return Ok(result);
    }
}