using MediatR;

namespace Application.Handlers.Institutions.Create;

public class CreateInstitutionRequest : IRequest<Guid>
{
    public string Name { get; set; } = null!;
    
    public string ContactEmail { get; set; } = null!;
    
    public string ClientId { get; set; } = null!;
}