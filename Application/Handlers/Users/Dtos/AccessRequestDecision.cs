namespace Application.Handlers.Users.Dtos;

public class AccessRequestDecision
{
    public int AccessRequestId { get; set; }
    
    public bool IsApproved { get; set; }

    public List<int>? ExcludedItemIds { get; set; } = [];
}