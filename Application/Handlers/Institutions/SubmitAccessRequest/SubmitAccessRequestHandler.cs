using Application.Exceptions;
using Application.Handlers.Institutions.Dtos;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.Institutions.SubmitAccessRequest;

public class SubmitAccessRequestHandler(IUserRepository userRepository, IInstitutionRepository institutionRepository, IDataItemRepository dataItemRepository,
    IAccessRequestRepository accessRequestRepository) : IRequestHandler<SubmitAccessRequest, AccessRequestDto>
{
    public async Task<AccessRequestDto> Handle(SubmitAccessRequest request, CancellationToken cancellationToken)
    {
        await ValidateRequest(request, cancellationToken);

        var accessRequest = new AccessRequest
        {
            InstitutionId = request.InstitutionId,
            UserId = request.UserId,
            RequestedAt = DateTime.UtcNow,
            RequestedCategory = request.Category,
            RequestedItemId = request.RequestedItemId,
        };
        
        await accessRequestRepository.CreateAsync(accessRequest, cancellationToken);
        
        return new AccessRequestDto()
        {
            InstitutionId = accessRequest.InstitutionId,
            UserId = accessRequest.UserId,
            RequestedItemId = accessRequest.RequestedItemId,
            RequestedCategory = accessRequest.RequestedCategory,
        };
    }

    private async Task ValidateRequest(SubmitAccessRequest request, CancellationToken cancellationToken)
    {
        if (request is { Category: not null, RequestedItemId: not null } ||
            (!request.Category.HasValue && !request.RequestedItemId.HasValue))
        {
            throw new BadRequestException("Either 'Category' or 'DataItemId' must be provided, but not both.");
        }
        
        var userExists = await userRepository.ExistsAsync(request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new NotFoundException($"User with id {request.UserId} was not found.");
        }
        
        var institutionExists = await institutionRepository.ExistsAsync(request.InstitutionId, cancellationToken);
        if (!institutionExists)
        {
            throw new NotFoundException($"Institution with id {request.InstitutionId} was not found.");
        }

        if (request.RequestedItemId.HasValue)
        {
            var dataItemExists = await dataItemRepository.ExistsAsync(request.RequestedItemId.Value, cancellationToken);

            if (!dataItemExists)
            {
                throw new NotFoundException($"Data item with id {request.RequestedItemId} was not found.");
            }
        }
    }
}