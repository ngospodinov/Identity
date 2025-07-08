using Application.Exceptions;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.AccessGrants.Create;

public class CreateAccessGrantHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateAccessGrantRequest, int>
{
    public async Task<int> Handle(CreateAccessGrantRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException($"User with id {request.UserId} not found.");
        }

        var accessGrant = new AccessGrant
        {
            UserId = request.UserId,
            ClientId = request.ClientId,
            Category = request.Category,
            GrantedAt = DateTime.UtcNow,
            };
        
        user.AccessGrants.Add(accessGrant);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return accessGrant.Id;
    }
}