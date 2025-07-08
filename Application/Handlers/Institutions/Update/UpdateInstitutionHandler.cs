using Application.Exceptions;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.Institutions.Update;

public class UpdateInstitutionHandler(IInstitutionRepository institutionRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateInstitutionRequest>
{
    public async Task Handle(UpdateInstitutionRequest request, CancellationToken cancellationToken)
    {
        var institution = await institutionRepository.GetInstitutionByIdAsync(request.InstitutionId, cancellationToken);

        if (institution is null)
        {
            throw new NotFoundException($"Institution with id: {request.InstitutionId} not found.");
        }
        
        institution.Name = request.Name;
        institution.ContactEmail = request.ContactEmail;
        institution.ClientId = request.ClientId;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}