using MediatR;

namespace Application.Handlers.AccessRequests.Review;

public class ReviewAccessRequest : IRequest
{
    public int AccessRequestId { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public bool IsApproved { get; set; } 

    public void SetId(int accessRequestId, Guid userId)
    {
        AccessRequestId = accessRequestId;
        UserId = userId;
    }
}