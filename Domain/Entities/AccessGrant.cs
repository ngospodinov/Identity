using Domain.Enums;

namespace Domain.Entities;

public class AccessGrant
{
    public int Id { get; set; }

    public string DataOwnerUserId { get; set; } = null!;
    
    public User DataOwnerUser { get; set; } = null!;
    
    public string RequesterUserId { get; set; } = null!;
    
    public User RequesterUser { get; set; } = null!;
    
    public DataCategory? Category { get; set; }
    
    [Obsolete("Grants are for categories/contexts. There will be specific revocations for items.")]
    public int? RequestedItemId { get; set; }

    public List<SpecificRevocation> SpecificRevocations { get; set; } = [];
    
    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? RevokedAt { get; set; } 
}