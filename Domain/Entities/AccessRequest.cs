using Domain.Enums;

namespace Domain.Entities;

public class AccessRequest
{
    public int Id { get; set; }
    
    public string DataOwnerUserId { get; set; } = string.Empty;
    
    public User DataOwnerUser { get; set; } = null!;
    
    public string RequesterUserId { get; set; } = string.Empty;
    
    public User RequesterUser { get; set; } = null!;
    
    public DataCategory? RequestedCategory { get; set; }
    
    [Obsolete("There won't be access requests for specific items.")]
    public int? RequestedItemId { get; set; }
    
    [Obsolete("There won't be access requests for specific items.")]

    public UserDataItem? RequestedItem { get; set; } 
    
    public DateTime RequestedAt { get; set; } 
    
    public DateTime? ReviewedAt { get; set; }

    public RequestStatus Status { get; set; } = RequestStatus.Pending;
}