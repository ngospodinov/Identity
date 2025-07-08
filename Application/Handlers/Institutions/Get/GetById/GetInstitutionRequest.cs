using Application.Handlers.Institutions.Dtos;
using MediatR;

namespace Application.Handlers.Institutions.Get.GetById;

public class GetInstitutionRequest(Guid institutionId) : IRequest<InstitutionDto>
{
    public Guid Id { get; set; } = institutionId;
}