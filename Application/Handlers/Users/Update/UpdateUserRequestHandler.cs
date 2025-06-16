using Application.Exceptions;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.Users.Update;

public class UpdateUserRequestHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserRequest>
{
    public async Task Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException($"User with id {request.UserId} not found.");
        }
        
        user.Email = request.Email;

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}