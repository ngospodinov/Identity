namespace IdentityProjectUI.Models;

public class AccessRequestDto
{
    public int Id { get; set; }
    
    public string RequesterUserId { get; set; } = string.Empty;
    
    public string RequesterEmail { get; set; } = string.Empty;
    
    public string RequesterUserName { get; set; } = string.Empty;
    
    public string Status { get; set; } = string.Empty;
    
    public string? RequestedCategory { get; set; } 
}