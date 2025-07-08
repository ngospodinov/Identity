using Application.Handlers.Institutions.Create;
using Application.Handlers.Institutions.Delete;
using Application.Handlers.Institutions.Get.GetById;
using Application.Handlers.Institutions.Get.GetPaged;
using Application.Handlers.Institutions.SubmitAccessRequest;
using Application.Handlers.Institutions.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstitutionController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateInstitutionAsync([FromBody] CreateInstitutionRequest request)
    {
        var id = await sender.Send(request);
        return CreatedAtRoute(nameof(GetInstitutionByIdAsync), new { id }, null);
    }

    [HttpGet]
    public async Task<IActionResult> GetInstitutionsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1)
    {
        return Ok(await sender.Send(new GetPagedInstitutionsRequest(pageSize, pageNumber)));
    }

    [HttpGet("{institutionId:guid}")]
    public async Task<IActionResult> GetInstitutionByIdAsync([FromRoute] Guid institutionId)
    {
        return Ok(await sender.Send(new GetInstitutionRequest(institutionId)));
    }

    [HttpPost("{institutionId:guid}")]
    public async Task<IActionResult> UpdateInstitutionAsync([FromRoute] Guid institutionId,
        [FromBody] UpdateInstitutionRequest request)
    {
        request.SetId(institutionId);
        await sender.Send(request);

        return NoContent();
    }

    [HttpDelete("{institutionId:guid}")]
    public async Task<IActionResult> DeleteInstitutionAsync([FromRoute] Guid institutionId)
    {
        await sender.Send(new DeleteInstitutionRequest(institutionId));
        
        return NoContent();
    }

    [HttpPost("{institutionId:guid}/access-requests")]
    public async Task<IActionResult> SubmitAccessRequest([FromRoute] Guid institutionId,
        [FromBody] SubmitAccessRequest request)
    {
        request.SetId(institutionId);
        var accessRequestDto = await sender.Send(request);
        
        var location = Url.Action("GetAccessRequestForUser", "User", new {
            userId = accessRequestDto.UserId,
            requestId = accessRequestDto.Id
        });
        
        return Created(location!, accessRequestDto);
    }
}