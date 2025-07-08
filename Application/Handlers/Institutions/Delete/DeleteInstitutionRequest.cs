using MediatR;

namespace Application.Handlers.Institutions.Delete;

public class DeleteInstitutionRequest(Guid institutionId) : IRequest
{
    public Guid InstitutionId { get; set; } = institutionId;
}