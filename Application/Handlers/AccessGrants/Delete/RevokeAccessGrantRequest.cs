using MediatR;

namespace Application.Handlers.AccessGrants.Delete;

public class RevokeAccessGrantRequest(string dataOwnerUserId, int grantId) : IRequest
{
    public string DataOwnerUserId { get; set; } = dataOwnerUserId;
    
    public int AccessGrantId { get; set; } = grantId;
}