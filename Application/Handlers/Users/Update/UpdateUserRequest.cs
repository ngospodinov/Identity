using MediatR;

namespace Application.Handlers.Users.Update;

public class UpdateUserRequest : IRequest
{
    public Guid UserId { get; private set; }
    
    public string Email { get; set; } = null!;

    public void SetId(Guid id)
    {
        UserId = id;
    }
}