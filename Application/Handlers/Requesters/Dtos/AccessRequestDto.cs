namespace Application.Handlers.Requesters.Dtos;

public class AccessRequestDto
{
    public int Id { get; set; }
    
    public string RequesterUserId { get;  set; } = null!;
    
    public string? RequesterEmail { get; set; } = string.Empty;
    
    public string? RequesterUserName { get; set; } = string.Empty;
    
    public string DataOwnerUserId { get; set; } = null!;
    
    public string? RequestedCategory { get; set; } 
    
    public int? RequestedItemId { get; set; }
    
    public DateTime RequestedAt { get; set; }
    
    public string Status { get; set; } = string.Empty;
}