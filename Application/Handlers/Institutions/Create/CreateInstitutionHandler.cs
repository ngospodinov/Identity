using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.Institutions.Create;

public class CreateInstitutionRequestHandler(IInstitutionRepository institutionRepository)
    : IRequestHandler<CreateInstitutionRequest, Guid>
{
    public async Task<Guid> Handle(CreateInstitutionRequest request, CancellationToken cancellationToken)
    {
        var institution = new Institution
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ContactEmail = request.ContactEmail,
            ClientId = request.ClientId
        };

        await institutionRepository.CreateInstitutionAsync(institution, cancellationToken);

        return institution.Id;
    }
}