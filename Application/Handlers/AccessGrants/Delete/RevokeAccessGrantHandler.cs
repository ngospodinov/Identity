using Application.Exceptions;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.AccessGrants.Delete;

public class RevokeAccessGrantHandler(IAccessGrantRepository accessGrantRepository, IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<RevokeAccessGrantRequest>
{
    public async Task Handle(RevokeAccessGrantRequest request, CancellationToken cancellationToken)
    {
        var userExists = await userRepository.ExistsAsync(request.DataOwnerUserId, cancellationToken);

        if (!userExists)
        {
            throw new NotFoundException ($"User with id {request.DataOwnerUserId} not found");
        }
        
        var accessGrant = await accessGrantRepository.GetAccessGrantByUserIdAsync(request.DataOwnerUserId, request.AccessGrantId, cancellationToken);
        if (accessGrant == null)
        {
            throw new NotFoundException($"Access grant with id ${request.AccessGrantId} for user with id {request.DataOwnerUserId} not found.");
        }
        
        accessGrant.RevokedAt = DateTime.UtcNow;
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}