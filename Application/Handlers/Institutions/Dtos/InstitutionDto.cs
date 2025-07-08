namespace Application.Handlers.Institutions.Dtos;

public class InstitutionDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string ContactEmail { get; set; } = null!;
    
    public string ClientId { get; set; } = null!;
}