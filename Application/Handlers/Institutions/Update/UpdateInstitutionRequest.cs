using MediatR;

namespace Application.Handlers.Institutions.Update;

public class UpdateInstitutionRequest : IRequest 
{
    public Guid InstitutionId { get; private set; }
    
    public string Name { get; set; } = null!;
    
    public string ContactEmail { get; set; } = null!;
    
    public string ClientId { get; set; } = null!;

    public void SetId(Guid id)
    {
        InstitutionId = id;
    }
}