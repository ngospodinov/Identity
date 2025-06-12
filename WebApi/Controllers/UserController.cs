using System.Net;
using Application.Handlers.Users;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController(ISender sender) : ControllerBase
{
    // [HttpGet("{id}")]
    // public async Task<ActionResult<UserEntity>> GetUserAsync(Guid id)
    // {
    //     var user = await _dbContext.Users
    //         .Include(x => x.DataItems)
    //         .FirstOrDefaultAsync(x => x.Id == id);
    //
    //     if (user == null)
    //         return NotFound();
    //
    //     return Ok(user);
    // }

    [HttpPost]
    [Authorize] 
    public async Task<ActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        var userId = await sender.Send(request);
        
        return StatusCode((int)HttpStatusCode.Created, userId);
    }

}