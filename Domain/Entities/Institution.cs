namespace Domain.Entities;

public class Institution
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Name { get; set; } = null!;
    
    public string ContactEmail { get; set; } = null!;
    
    public string ClientId { get; set; } = null!;

    public ICollection<AccessGrant> AccessGrants { get; set; } = new List<AccessGrant>();
    
    public ICollection<AccessRequest> AccessRequests { get; set; } = new List<AccessRequest>();
}