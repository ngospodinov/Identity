using Domain.Enums;

namespace Application.Handlers.Users.Dtos;

public class AccessGrantDto
{
    public int Id { get; set; }
    
    public Guid InstitutionId { get; set; }  

    public DataCategory Category { get; set; }

    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
}