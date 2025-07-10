using Application.Common.Models;
using Application.Handlers.Institutions.Dtos;
using MediatR;

namespace Application.Handlers.AccessRequests.Get.GetPaged;

public class GetPagedAccessRequest(Guid userId, Guid? institutionId, int pageSize, int pageNumber) : IRequest<PagedResult<AccessRequestDto>>
{
    public Guid UserId { get; set; } = userId;
    
    public Guid? InstitutionId { get; set; } = institutionId;
    
    public int PageSize { get; set; } = pageSize;
    
    public int PageNumber { get; set; } = pageNumber;
}