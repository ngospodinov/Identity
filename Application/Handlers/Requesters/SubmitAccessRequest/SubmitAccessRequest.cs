using Application.Handlers.Requesters.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.Requesters.SubmitAccessRequest;

public class SubmitAccessRequest : IRequest<AccessRequestDto>
{
    public string RequesterUserId { get; private set; } = string.Empty;
    
    public string DataOwnerUserId { get; set; } = string.Empty;
    
    public DataCategory? Category { get; set; } 
    
    public int? RequestedItemId { get; set; }

    public void SetId(string requesterUserId)
    {
        RequesterUserId = requesterUserId;
    }
}