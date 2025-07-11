using Application.Exceptions;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.Users.DataItems.Create;

public class CreateDataItemHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateDataItemRequest, int>
{
    public async Task<int> Handle(CreateDataItemRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException($"User with id {request.UserId} not found.");
        }

        var newItem = new UserDataItem
        {
            UserId = request.UserId,
            Key = request.Key,
            Value = request.Value,
            Category = request.Category,
        };
        
        user.DataItems.Add(newItem);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return newItem.Id;
    }
}