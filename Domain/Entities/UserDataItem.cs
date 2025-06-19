using Domain.Enums;

namespace Domain.Entities;

public class UserDataItem
{
    public int Id { get; set; } 
    
    public string UserId { get; set; } = null!;
    
    public User User { get; set; } = null!;

    public string Key { get; set; } = null!;
    
    public string Value { get; set; } = null!;
    
    public DataCategory Category { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsDeleted { get; set; } = false;
    
    public DateTime? DeletedAt { get; set; }

    public List<SpecificRevocation> SpecificRevocations { get; set; } = [];
}