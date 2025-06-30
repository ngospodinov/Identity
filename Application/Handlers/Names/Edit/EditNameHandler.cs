using Application.Common;
using Application.Exceptions;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.Names.Edit;

public class EditNameHandler(INameRepository nameRepository, IUnitOfWork unitOfWork) : IRequestHandler<EditNameRequest>
{
    public async Task Handle(EditNameRequest request, CancellationToken cancellationToken)
    {
       var name =  await nameRepository.GetByIdAsync(request.Id, request.UserId, cancellationToken);

       if (name is null)
       {
           throw new NotFoundException($"Name with id {request.Id} for user with id {request.UserId} could not be found.");
       }
       
       if (request.IsDefaultForCategory)
       {
           await nameRepository.UnsetDefaultsAsync(request.UserId, [request.Category.ParseDataCategory()], cancellationToken);
       }
       
       name.FirstName = request.FirstName;
       name.MiddleName = request.MiddleName;
       name.LastName = request.LastName;
       name.Category = request.Category.ParseDataCategory();
       name.IsDefaultForCategory = request.IsDefaultForCategory;

       
       await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}