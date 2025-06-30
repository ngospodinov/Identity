using Application.Common;
using Application.Exceptions;
using Application.Handlers.Users.Dtos;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.DataItems.Get.GetPaged;

public class GetPagedDataItemsHandler(IUserRepository userRepository, IDataItemRepository dataItemRepository) : IRequestHandler<GetPagedDataItemsRequest, PagedResult<UserDataItemDto>>
{
    public async Task<PagedResult<UserDataItemDto>> Handle(GetPagedDataItemsRequest request,
        CancellationToken cancellationToken)
    {
        var userExists = await userRepository.ExistsAsync(request.DataOwnerUserId, cancellationToken);

        if (!userExists)
        {
            throw new NotFoundException($"User with id {request.DataOwnerUserId} not found.");
        }
        
        var dataItems = await dataItemRepository.GetUserDataItemsAsync(request.DataOwnerUserId, request.Category, request.PageNumber, request.PageSize, cancellationToken);

        return new PagedResult<UserDataItemDto>
        {
            Items = dataItems,
            TotalCount = await dataItemRepository.CountAsync(request.DataOwnerUserId, cancellationToken),
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
        };
    }
}