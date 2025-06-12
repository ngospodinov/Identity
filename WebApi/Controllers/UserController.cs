using System.Net;
using Application.Handlers.Users.Create;
using Application.Handlers.Users.Dtos;
using Application.Handlers.Users.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController(ISender sender) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserAsync(Guid userId)
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

}