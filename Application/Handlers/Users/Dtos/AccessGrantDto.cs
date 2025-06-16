using Domain.Enums;

namespace Application.Handlers.Users.Dtos;

public class AccessGrantDto
{
    public int Id { get; set; }
    
    public string? RequesterUserId { get; set; } 
    
    public string? DataOwnerUserId { get; set; }

    public string Category { get; set; } = null!;
    
    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? RevokedAt { get; set; }
}