using Application.Exceptions;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.Users.DataItems.Delete;

public class DeleteDataItemHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteDataItemRequest>
{
    public async Task Handle(DeleteDataItemRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException($"User with id ${request.UserId} not found.");
        }
        
        var item = user.DataItems.FirstOrDefault(x => x.Id == request.DataItemId);
        if (item == null)
        {
            throw new NotFoundException($"Item with id ${request.DataItemId} not found.");
        }
        
        user.DataItems.Remove(item);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}