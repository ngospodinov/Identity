using Domain.Enums;

namespace Domain.Entities;

public class UserDataItem
{
    public int Id { get; set; } 
    
    public Guid UserId { get; set; }
    
    public UserEntity User { get; set; } = null!;

    public string Key { get; set; } = null!;
    
    public string Value { get; set; } = null!;
    
    public DataCategory Category { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}