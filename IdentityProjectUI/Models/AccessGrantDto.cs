namespace IdentityProjectUI.Models;

public class AccessGrantDto
{
    public int Id { get; set; }
    
    public string RequesterUserId { get; set; } = null!;
    
    public string RequesterEmail { get; set; } = null!;
    
    public string Category { get; set; } = null!;
    
    public DateTime GrantedAt { get; set; }
    
    public DateTime? RevokedAt { get; set; }
}