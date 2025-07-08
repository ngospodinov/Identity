using Application.Exceptions;
using Application.Handlers.Institutions.Dtos;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.Institutions.Get.GetById;

public class GetInstitutionHandler(IInstitutionRepository institutionRepository) : IRequestHandler<GetInstitutionRequest, InstitutionDto>
{
    public async Task<InstitutionDto> Handle(GetInstitutionRequest request, CancellationToken cancellationToken)
    {
        var institution = await institutionRepository.GetInstitutionByIdAsync(request.Id, cancellationToken);

        if (institution == null)
        {
            throw new NotFoundException($"Institution with id: {request.Id} not found.");
        }
        
        return new InstitutionDto()
        {
            Id = institution.Id,
            Name = institution.Name,
            ClientId = institution.ClientId,
            ContactEmail = institution.ContactEmail,
        };
    }
}