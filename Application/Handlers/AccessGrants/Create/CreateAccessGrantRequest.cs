using Domain.Enums;
using MediatR;

namespace Application.Handlers.AccessGrants.Create;

public class CreateAccessGrantRequest() : IRequest<int>
{
    public string DataOwnerUserId { get; private set; } = default!;
    
    public string RequesterUserId { get; private set; } = default!;
    
    public DataCategory? Category { get; set; }
    
    public int? DataItemId { get; set; }

    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;

    public void SetId(string userId)
    {
        DataOwnerUserId = userId;
    }
}