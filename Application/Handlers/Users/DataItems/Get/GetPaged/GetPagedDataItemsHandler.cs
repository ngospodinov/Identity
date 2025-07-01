using Application.Exceptions;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.Users.DataItems.Get.GetPaged;

public class GetPagedDataItemsHandler(IUserRepository userRepository, IDataItemRepository dataItemRepository) : IRequestHandler<GetPagedDataItemsRequest, GetPagedDataItemsResponse>
{
    public async Task<GetPagedDataItemsResponse> Handle(GetPagedDataItemsRequest request,
        CancellationToken cancellationToken)
    {
        var userExists = await userRepository.ExistsAsync(request.UserId, cancellationToken);

        if (!userExists)
        {
            throw new NotFoundException($"User with id {request.UserId} not found.");
        }
        
        var dataItems = await dataItemRepository.GetUserDataItemsAsync(request.UserId, request.Category, request.PageNumber, request.PageSize, cancellationToken);

        return new GetPagedDataItemsResponse
        {
            DataItems = dataItems,
            TotalCount = dataItems.Count,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
        };
    }
}