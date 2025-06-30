using Application.Exceptions;
using Application.Handlers.Users.Dtos;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.DataItems.Get.GetById;

public class GetDataItemHandler(IDataItemRepository dataItemRepository, IUserRepository userRepository) : IRequestHandler<GetDataItemRequest, UserDataItemDto?>
{
    public async Task<UserDataItemDto?> Handle(GetDataItemRequest request, CancellationToken cancellationToken)
    {
        var userExists = await userRepository.ExistsAsync(request.UserId, cancellationToken);

        if (!userExists)
        {
            throw new NotFoundException($"User with id {request.UserId} not found.");
        }
        
        return await dataItemRepository.GetDataItemByIdAsync(request.UserId, request.Id, cancellationToken);
    }
}