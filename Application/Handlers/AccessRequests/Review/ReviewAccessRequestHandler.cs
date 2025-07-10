using Application.Exceptions;
using Application.Handlers.AccessGrants.Create;
using Application.Repositories;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.AccessRequests.Review;

public class ReviewAccessRequestHandler(
    IUserRepository userRepository,
    IAccessRequestRepository accessRequestRepository,
    IAccessGrantRepository accessGrantRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ReviewAccessRequest>
{
    public async Task Handle(ReviewAccessRequest request, CancellationToken cancellationToken)
    {
        var userExists = await userRepository.ExistsAsync(request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new NotFoundException($"User with id {request.UserId} was not found.");
        }
        
        var accessRequestExists = await accessRequestRepository.ExistsAsync(request.AccessRequestId, cancellationToken);
        if (!accessRequestExists)
        {
            throw new NotFoundException($"Access request with id {request.AccessRequestId} was not found.");
        }
        
        var accessRequest = await accessRequestRepository.GetEntityAsync(request.AccessRequestId, cancellationToken);
        accessRequest!.Status = request.IsApproved? RequestStatus.Approved : RequestStatus.Denied;
        accessRequest.ReviewedAt = DateTime.UtcNow;
        
        if (request.IsApproved)
        {
            var accessGrantExists = await accessGrantRepository.ExistsAsync(accessRequest.UserId, accessRequest.InstitutionId,
                accessRequest.RequestedCategory, accessRequest.RequestedItemId, cancellationToken);
            
            if (!accessGrantExists)
            {
                await CreateAccessGrant(accessRequest, cancellationToken);
            }
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task CreateAccessGrant(AccessRequest accessRequest, CancellationToken cancellationToken)
    {
        var accessGrant = new AccessGrant
        {
            UserId = accessRequest.UserId,
            InstitutionId = accessRequest.InstitutionId,
            Category = accessRequest.RequestedCategory,
            GrantedAt = DateTime.UtcNow,
            RequestedItemId = accessRequest.RequestedItemId,
        };
        
        await accessGrantRepository.CreateAccessGrantAsync(accessGrant, cancellationToken);
    }
}