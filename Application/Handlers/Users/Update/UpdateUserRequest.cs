using MediatR;

namespace Application.Handlers.Users.Update;

public class UpdateUserRequest : IRequest
{
    public string UserId { get; private set; } = null!;
    
    public string Email { get; set; } = null!;

    public void SetId(string id)
    {
        UserId = id;
    }
}