using System.Text.Json.Serialization;
using MediatR;

namespace Application.Handlers.AccessRequests.Review;

public class ReviewAccessRequest : IRequest
{
    public int AccessRequestId { get; set; }
 
    public string DataOwnerUserId { get; set; }
    
    public bool IsApproved { get; set; }

    public List<int> ExcludedItemIds { get; set; } = [];
}