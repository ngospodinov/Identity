using Application.Common;
using Application.Exceptions;
using Application.Handlers.Users.Dtos;
using Application.Repositories;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.Users.Data;

public class GetUserDataHandler(IUserRepository userRepository, IDataItemRepository dataItemRepository,
    IAccessGrantRepository accessGrantRepository) : IRequestHandler<GetUserDataRequest, List<UserDataItemDto>>
{
    public async Task<List<UserDataItemDto>> Handle(GetUserDataRequest request, CancellationToken cancellationToken)
    {
        var userExists = await userRepository.ExistsAsync(request.DataOwnerUserId, cancellationToken);

        if (!userExists)
        {
            throw new NotFoundException($"User with id {request.DataOwnerUserId} not found.");
        }
        
        var accessGrant = await accessGrantRepository.GetActiveAccessGrantForCategoryAsync(request.DataOwnerUserId, request.RequesterUserId, 
           request.Category.ParseDataCategory(), cancellationToken);
        if (accessGrant is null)
        {
            throw new Exception($"Access not granted for category: {request.Category}");
        }
        
        var dataItems = await dataItemRepository.GetUserDataItemsForCategoryAsync(
            request.DataOwnerUserId,
            request.Category.ParseDataCategory(),
            cancellationToken);
        
        var excludeIds = accessGrant.SpecificRevocations
            .Select(x => x.UserDataItemId)
            .ToHashSet(); 
    
        var filteredDataItems = dataItems
            .Where(x => !excludeIds.Contains(x.Id))
            .ToList();
        
        return filteredDataItems;
    }
    
}