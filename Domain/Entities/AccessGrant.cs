using Domain.Enums;

namespace Domain.Entities;

public class AccessGrant
{
    public int Id { get; set; }

    public Guid UserId { get; set; }
    
    public User User { get; set; } = null!;
    
    public string ClientId { get; set; } = null!;

    public Guid InstitutionId { get; set; } 
    
    public Institution Institution { get; set; } = null!;

    public DataCategory Category { get; set; }

    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? RevokedAt { get; set; } 
}