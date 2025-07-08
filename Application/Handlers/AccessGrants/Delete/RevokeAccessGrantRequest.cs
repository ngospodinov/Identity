using MediatR;

namespace Application.Handlers.AccessGrants.Delete;

public class RevokeAccessGrantRequest(Guid userId, int grantId) : IRequest
{
    public Guid UserId { get; set; } = userId;
    
    public int AccessGrantId { get; set; } = grantId;
}