namespace IdentityProvider.Models;

public class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Type { get; set; } = default!;      
    
    public string Payload { get; set; } = default!;
    
    
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    
    
    public DateTime? ProcessedOnUtc { get; set; }
    
    public string? Error { get; set; }
}