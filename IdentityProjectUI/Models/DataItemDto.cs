namespace IdentityProjectUI.Models;

public class DataItemDto
{
    public int Id { get; set; }
    
    public string Category { get; set; } = null!;
    
    public string Key { get; set; } = null!;
    
    public string? Value { get; set; }
}