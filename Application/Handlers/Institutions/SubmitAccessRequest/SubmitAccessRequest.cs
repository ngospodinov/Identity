using Application.Handlers.Institutions.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.Institutions.SubmitAccessRequest;

public class SubmitAccessRequest : IRequest<AccessRequestDto>
{
    public Guid InstitutionId { get; private set; } 
    
    public Guid UserId { get; set; }
    
    public DataCategory? Category { get; set; } 
    
    public int? RequestedItemId { get; set; }

    public void SetId(Guid institutionId)
    {
        InstitutionId = institutionId;
    }
}