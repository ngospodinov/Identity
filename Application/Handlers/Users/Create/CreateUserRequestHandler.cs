using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.Users.Create;

public class CreateUserRequestHandler(IUserRepository userRepository) : IRequestHandler<CreateUserRequest, Guid>
{
    public async Task<Guid> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var newUser = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
        };
        
        await userRepository.CreateUserAsync(newUser, cancellationToken);
        
        return newUser.Id;
    }
}