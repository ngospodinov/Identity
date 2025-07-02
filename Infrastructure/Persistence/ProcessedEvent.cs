namespace Infrastructure.Persistence;

public class ProcessedEvent
{
    public Guid Id { get; set; }   
    
    public DateTime ProcessedUtc { get; set; }
}