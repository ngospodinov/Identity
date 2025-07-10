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
        
        var user = await userRepository.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException($"User with id {request.UserId} not found.");
        }

        var accessGrant = new AccessGrant
        {
            UserId = request.UserId,
            InstitutionId = request.InstitutionId,
            Category = request.Category,
            RequestedItemId = request.DataItemId,
            GrantedAt = DateTime.UtcNow,
            };
        
        user.AccessGrants.Add(accessGrant);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return accessGrant.Id;
    }
}