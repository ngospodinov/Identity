using Application.Exceptions;
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
        var userExists = await userRepository.ExistsAsync(request.DataOwnerUserId, cancellationToken);
        if (!userExists)
        {
            throw new NotFoundException($"User with id {request.DataOwnerUserId} was not found.");
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
            var accessGrantExists = await accessGrantRepository.ExistsAsync(accessRequest.DataOwnerUserId, accessRequest.RequesterUserId,
                accessRequest.RequestedCategory, accessRequest.RequestedItemId, cancellationToken);
            
            if (!accessGrantExists)
            {
                await CreateAccessGrant(accessRequest, request.ExcludedItemIds, cancellationToken);
            }
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task CreateAccessGrant(AccessRequest accessRequest, List<int> excludedIds, CancellationToken cancellationToken)
    {
        var accessGrant = new AccessGrant
        {
            DataOwnerUserId = accessRequest.DataOwnerUserId,
            RequesterUserId = accessRequest.RequesterUserId,
            Category = accessRequest.RequestedCategory,
            GrantedAt = DateTime.UtcNow,
        };

        if (excludedIds.Count > 0)
        {
            accessGrant.SpecificRevocations.AddRange(
                excludedIds.Distinct().Select(id => new SpecificRevocation
                {
                    UserDataItemId = id
                }).ToList());
        }
        
        await accessGrantRepository.CreateAccessGrantAsync(accessGrant, cancellationToken);
    }
}