using Application.Exceptions;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.AccessGrants.Create;

public class CreateAccessGrantHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateAccessGrantRequest, int>
{
    public async Task<int> Handle(CreateAccessGrantRequest request, CancellationToken cancellationToken)
    {
        if (request is { Category: not null, DataItemId: not null } ||
            (!request.Category.HasValue && !request.DataItemId.HasValue))
        {
            throw new BadRequestException("Either 'Category' or 'DataItemId' must be provided, but not both.");
        }
        
        var user = await userRepository.GetUserByIdAsync(request.DataOwnerUserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException($"User with id {request.DataOwnerUserId} not found.");
        }

        var accessGrant = new AccessGrant
        {
            DataOwnerUserId = request.DataOwnerUserId,
            RequesterUserId = request.RequesterUserId,
            Category = request.Category,
            RequestedItemId = request.DataItemId,
            GrantedAt = DateTime.UtcNow,
            };
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return accessGrant.Id;
    }
}