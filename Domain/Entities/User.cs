namespace Domain.Entities;

public class User 
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string UserName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } 
    
    public bool IsDeleted { get; set; } = false;
    
    public DateTime? DeletedAt { get; set; } 
    
    public ICollection<UserDataItem> DataItems { get; set; } = [];
}