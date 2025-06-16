using MediatR;

namespace Application.Handlers.Users.Create;

public class CreateUserRequest : IRequest<string>
{
    public string Username { get; set; } = null!;
    
    public string Email { get; set; } = null!;
}