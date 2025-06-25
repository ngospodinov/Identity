using Application.Common;
using Application.Handlers.Names.Create;
using Application.Handlers.Names.Delete;
using Application.Handlers.Names.Edit;
using Application.Handlers.Names.GetById;
using Application.Handlers.Names.GetMyNames;
using Application.Handlers.Names.GetNameByUser;
using Application.Handlers.Users.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class NameController(ISender sender) : ControllerBase
{
    [HttpGet("me")]
    public async Task<PagedResult<NameDto>> GetMine(CancellationToken ct, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        return await sender.Send(new GetMyNamesRequest(User.FindFirst("sub")!.Value, pageSize, pageNumber), ct);
    }
    
    [HttpPost("me")]
    public async Task<IActionResult> CreateName([FromBody] NameDto nameDto, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")!.Value;

        var request = new CreateNameRequest
        {
            FirstName = nameDto.FirstName,
            MiddleName = nameDto.MiddleName,
            LastName = nameDto.LastName,
            Category = nameDto.Category,
            IsDefaultForCategory = nameDto.IsDefaultForCategory,
            UserId = userId
        };
            
        await sender.Send(request, ct);
        return NoContent();
    }

    [HttpGet("me/{id:int}")]
    public async Task<NameDto> GetNameAsync([FromRoute] int id, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")!.Value;

        return await sender.Send(new GetNameRequest(id, userId), ct);
    }

    [HttpGet("{userId}")]
    public async Task<NameDto> GetNameByUserIdAsync([FromRoute] string userId, [FromQuery] string category, CancellationToken ct)
    {
        return await sender.Send(new GetNameByUserRequest(userId, category), ct);
    }
    
    [HttpPost("me/{id:int}")]
    public async Task<IActionResult> EditName([FromRoute] int id, [FromBody] NameDto nameDto, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")!.Value;

        var request = new EditNameRequest
        {
            UserId = userId,
            Id = id,
            FirstName = nameDto.FirstName,
            MiddleName = nameDto.MiddleName,
            LastName = nameDto.LastName,
            Category = nameDto.Category,
            IsDefaultForCategory = nameDto.IsDefaultForCategory,
        };
            
        await sender.Send(request, ct);
        return NoContent();
    }

    [HttpDelete("me/{id:int}")]
    public async Task<IActionResult> DeleteName([FromRoute] int id, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")!.Value;
        
        await sender.Send(new DeleteNameRequest(id, userId), ct);
        return NoContent();
    }
}