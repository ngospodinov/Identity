using Application.Exceptions;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.Institutions.Delete;

public class DeleteInstitutionHandler(IInstitutionRepository institutionRepository) : IRequestHandler<DeleteInstitutionRequest>
{
    public async Task Handle(DeleteInstitutionRequest request, CancellationToken cancellationToken)
    {
        var institutionExists = await institutionRepository.ExistsAsync(request.InstitutionId, cancellationToken);

        if (!institutionExists)
        {
            throw new NotFoundException($"Institution with id {request.InstitutionId} not found.");
        }
        
        await institutionRepository.DeleteInstitutionAsync(request.InstitutionId, cancellationToken);
    }
}