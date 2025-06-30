using Application.Exceptions;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.DataItems.Delete;

public class DeleteDataItemHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteDataItemRequest>
{
    public async Task Handle(DeleteDataItemRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.DataOwnerUserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException($"User with id ${request.DataOwnerUserId} not found.");
        }
        
        var item = user.DataItems.FirstOrDefault(x => x.Id == request.DataItemId);
        if (item == null)
        {
            throw new NotFoundException($"Item with id ${request.DataItemId} not found.");
        }

        item.DeletedAt = DateTime.UtcNow;
        item.IsDeleted = true;
        
        foreach (var sr in item.SpecificRevocations)
        {
            sr.IsDeleted = true;
            sr.DeletedAt = DateTime.UtcNow;
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}