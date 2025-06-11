using MediatR;

namespace Application.Handlers.Users;

public class CreateUserRequest : IRequest<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Username { get; set; } = null!;
    
    public string Email { get; set; } = null!;
}