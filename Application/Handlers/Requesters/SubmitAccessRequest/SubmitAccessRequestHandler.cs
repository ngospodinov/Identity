using Application.Exceptions;
using Application.Handlers.Requesters.Dtos;
using Application.Repositories;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.Requesters.SubmitAccessRequest;

public class SubmitAccessRequestHandler(IUserRepository userRepository, IDataItemRepository dataItemRepository,
    IAccessRequestRepository accessRequestRepository, ICurrentClientProvider currentClientProvider, IUnitOfWork unitOfWork) : IRequestHandler<SubmitAccessRequest, AccessRequestDto>
{
    public async Task<AccessRequestDto> Handle(SubmitAccessRequest request, CancellationToken cancellationToken)
    {
        await ValidateRequest(request, cancellationToken);

        var accessRequest = new AccessRequest
        {
            RequesterUserId = request.RequesterUserId,
            DataOwnerUserId = request.DataOwnerUserId,
            RequestedAt = DateTime.UtcNow,
            RequestedCategory = request.Category,
            RequestedItemId = request.RequestedItemId,
        };
        
        await accessRequestRepository.CreateAsync(accessRequest, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new AccessRequestDto()
        {
            Id = accessRequest.Id,
            RequesterUserId = accessRequest.RequesterUserId,
            DataOwnerUserId = accessRequest.DataOwnerUserId,
            RequestedItemId = accessRequest.RequestedItemId,
            RequestedCategory = accessRequest.RequestedCategory?.ParseCategory(),
            Status = RequestStatus.Pending.ParseStatus(),
        };
    }

    private async Task ValidateRequest(SubmitAccessRequest request, CancellationToken cancellationToken)
    {
        var currentClient = currentClientProvider.GetCurrentClientId();
        if (string.IsNullOrEmpty(currentClient))
        {
            throw new UnauthorizedAccessException("Missing client_id in token.");
        }
        
        if (request is { Category: not null, RequestedItemId: not null } ||
            (!request.Category.HasValue && !request.RequestedItemId.HasValue))
        {
            throw new BadRequestException("Either 'Category' or 'DataItemId' must be provided, but not both.");
        }
        
        var userExists = await userRepository.ExistsAsync(request.DataOwnerUserId, cancellationToken);
        if (!userExists)
        {
            throw new NotFoundException($"User with id {request.DataOwnerUserId} was not found.");
        }
        
        var requesterExists = await userRepository.ExistsAsync(request.RequesterUserId, cancellationToken);
        if (!requesterExists)
        {
            throw new NotFoundException($"Institution with id {request.RequesterUserId} was not found.");
        }

        // string? requiredScope = null;
        
        if (request.RequestedItemId.HasValue)
        {
            var dataItemCategory = await dataItemRepository.GetCategoryAsync(request.RequestedItemId.Value, cancellationToken);

            if (dataItemCategory == null)
            {
                throw new NotFoundException($"Data item with id {request.RequestedItemId} was not found.");
            }
            
            // requiredScope = GetScopeForCategory(dataItemCategory.Value);
        }
        // else if (request.Category.HasValue)
        // {
        //    requiredScope = GetScopeForCategory(request.Category.Value);
        // }
        //
        // var hasScope = currentClientProvider.HasScope(requiredScope!);
        // if (!hasScope)
        // {
        //     throw new UnauthorizedAccessException($"Institution does not have the necessary scope: {requiredScope}");
        // }
    }
    
    // private static string GetScopeForCategory(DataCategory category)
    // {
    //     return category switch
    //     {
    //         DataCategory.Financial => "financial.read",
    //         DataCategory.Personal => "personal.read",
    //         DataCategory.Academic => "academic.read",
    //         DataCategory.Legal => "legal.read",
    //         DataCategory.General => "general.read",
    //         _ => throw new InvalidOperationException("Scope not defined for this category.")
    //     };
    // }
}