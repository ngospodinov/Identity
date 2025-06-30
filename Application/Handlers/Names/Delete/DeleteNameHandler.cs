using Application.Exceptions;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.Names.Delete;

public class DeleteNameHandler(INameRepository nameRepository) : IRequestHandler<DeleteNameRequest>
{
    public async Task Handle(DeleteNameRequest request, CancellationToken cancellationToken)
    {
        var name = await nameRepository.GetByIdAsync(request.Id, request.UserId, cancellationToken);

        if (name == null)
        {
            throw new NotFoundException($"Name with id {request.Id} for user with id {request.UserId} could not be found.");
        }
        
        name.IsDeleted = true;
        name.DeletedAt = DateTime.UtcNow;
        
        await nameRepository.SaveChangesAsync(cancellationToken);
    }
}