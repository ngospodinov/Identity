using Application.Handlers.AccessGrants.Get.GetPaged.Requester;
using Application.Handlers.AccessRequests.Get.GetPaged.Requester;
using Application.Handlers.Requesters.SubmitAccessRequest;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]

public class InstitutionController(ISender sender) : ControllerBase
{
    [HttpPost("{requesterId}/access-requests")]
    public async Task<IActionResult> SubmitAccessRequest([FromRoute] string requesterId,
        [FromBody] SubmitAccessRequest request)
    {
        request.SetId(requesterId);
        var accessRequestDto = await sender.Send(request);

        var location = Url.Action("GetAccessRequestForUser", "User", new
        {
            userId = accessRequestDto.DataOwnerUserId,
            requestId = accessRequestDto.Id
        });

        return Created(location!, accessRequestDto);
    }

    [HttpGet("{requesterId}/access-requests")]
    public async Task<IActionResult> GetAccessRequest([FromRoute] string requesterId, [FromQuery] string? dataOwnerUserId,
        [FromQuery] DataCategory? category,
        [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        return Ok(await sender.Send(new GetPagedAccessRequestsByRequesterRequest(requesterId, dataOwnerUserId, category, pageSize,
            pageNumber)));
    }

    [HttpGet("{requesterId}/access-grants", Name = "GetInstitutionAccessGrants")]
    public async Task<IActionResult> GetAccessGrantsAsync([FromRoute] string requesterId, [FromQuery] string? dataOwnerUserId,
        [FromQuery] DataCategory? category, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        return Ok(await sender.Send(
            new GetPagedAccessGrantsByRequesterRequest(requesterId, dataOwnerUserId, category, pageSize, pageNumber)));
    }
}