using Domain.Enums;
using MediatR;

namespace Application.Handlers.AccessGrants.Create;

public class CreateAccessGrantRequest() : IRequest<int>
{
    public Guid UserId { get; private set; }
    
    public string ClientId { get; set; } = null!; 

    public DataCategory Category { get; set; }

    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;

    public void SetId(Guid userId)
    {
        UserId = userId;
    }
}