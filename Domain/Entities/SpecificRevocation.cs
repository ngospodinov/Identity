namespace Domain.Entities;

public class SpecificRevocation
{
    public int Id { get; set; }
    
    public int AccessGrantId { get; set; }
    public AccessGrant AccessGrant { get; set; } = null!;
    
    public int UserDataItemId { get; set; } 
    public UserDataItem UserDataItem { get; set; } = null!;
    
    public bool IsDeleted { get; set; } = false;
    
    public DateTime? DeletedAt { get; set; }
}