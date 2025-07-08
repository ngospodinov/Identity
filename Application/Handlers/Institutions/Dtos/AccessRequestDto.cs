using Domain.Enums;

namespace Application.Handlers.Institutions.Dtos;

public class AccessRequestDto
{
    public int Id { get; set; }
    
    public Guid InstitutionId { get;  set; } 
    
    public Guid UserId { get; set; }
    
    public DataCategory? RequestedCategory { get; set; } 
    
    public int? RequestedItemId { get; set; }
}