using Domain.Enums;

namespace Domain.Entities;

public class AccessRequest
{
    public int Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public User User { get; set; } = null!;
    
    public Guid InstitutionId { get; set; }
    
    public Institution Institution { get; set; } = null!;
    
    public DataCategory? RequestedCategory { get; set; }
    
    public int? RequestedItemId { get; set; }
    
    public UserDataItem? RequestedItem { get; set; } = null!;
    
    public DateTime RequestedAt { get; set; } 
    
    public DateTime ReviewedAt { get; set; }

    public RequestStatus Status { get; set; } = RequestStatus.Pending;
}