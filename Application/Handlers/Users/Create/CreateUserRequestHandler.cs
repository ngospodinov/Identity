using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.Users.Create;

public class CreateUserRequestHandler(IUserRepository userRepository) : IRequestHandler<CreateUserRequest, string>
{
    public async Task<string> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var newUser = new User()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.Username,
            Email = request.Email,
        };
        
        await userRepository.CreateUserAsync(newUser, cancellationToken);
        
        return newUser.Id;
    }
}