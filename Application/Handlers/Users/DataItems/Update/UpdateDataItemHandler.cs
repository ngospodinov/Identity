using Application.Exceptions;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.Users.DataItems.Update;

public class UpdateDataItemHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateDataItemRequest, int>
{
    public async Task<int> Handle(UpdateDataItemRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"User with id {request.UserId} not found.");
        }

        var item = user.DataItems.FirstOrDefault(x => x.Id == request.DataItemId);
        if (item == null)
        {
            throw new NotFoundException($"Item with id {request.DataItemId} not found.");
        }
        
        item.Value = request.Value;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return item.Id;
    }
}