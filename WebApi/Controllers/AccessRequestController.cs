using Application.Common;
using Application.Handlers.AccessRequests.Create;
using Application.Handlers.AccessRequests.Get.GetById;
using Application.Handlers.AccessRequests.Get.GetPaged.User;
using Application.Handlers.AccessRequests.Review;
using Application.Handlers.Users.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/access-requests")]
public class AccessRequestController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostAccessRequestAsync([FromBody] NewAccessRequestsDto newAccessRequestsDto, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")!.Value;

        await sender.Send(new CreateAccessRequests
        {
            RequesterUserId = userId,
            DataOwnerUserId = newAccessRequestsDto.TargetUserId,
            SelectedCategories = newAccessRequestsDto.SelectedCategories.Select(x => x.ParseDataCategory()).ToList()
        }, ct);
        
        return NoContent();
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> GetMyAccessRequests(CancellationToken ct, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        var userId = User.FindFirst("sub")!.Value;

        return Ok(await sender.Send(new GetPagedAccessRequest(userId, pageSize, pageNumber), ct));
    }
    
    [HttpGet("{userId}/access-requests/{accessRequestId:int}", Name = "GetAccessRequestForUser")]
    public async Task<IActionResult> GetAccessRequestForUser([FromRoute] string userId, [FromRoute] int accessRequestId)
    {
        return Ok(await sender.Send(new GetAccessRequestByIdRequest(userId, accessRequestId)));
    }

    [HttpPost("decision")]
    public async Task<IActionResult> HandleDecision([FromBody] AccessRequestDecision decision, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")!.Value;
        
        var request = new ReviewAccessRequest()
        {
            AccessRequestId = decision.AccessRequestId,
            DataOwnerUserId = userId,
            IsApproved = decision.IsApproved,
            ExcludedItemIds = decision.ExcludedItemIds ?? [],
        };
        
        await sender.Send(request, ct);

        return NoContent();
    }
}