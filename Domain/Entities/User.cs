namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Username { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserDataItem> DataItems { get; set; } = [];
    
    public ICollection<AccessGrant> AccessGrants { get; set; } = [];
    
    public ICollection<AccessRequest> AccessRequests { get; set; } = [];
}