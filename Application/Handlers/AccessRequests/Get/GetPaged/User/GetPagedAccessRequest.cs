using Application.Common;
using Application.Handlers.Requesters.Dtos;
using MediatR;

namespace Application.Handlers.AccessRequests.Get.GetPaged.User;

public class GetPagedAccessRequest(string dataOwnerUserId,  int pageSize, int pageNumber) : IRequest<PagedResult<AccessRequestDto>>
{
    public string DataOwnerUserId { get; set; } = dataOwnerUserId;
    
    public int PageSize { get; set; } = pageSize;
    
    public int PageNumber { get; set; } = pageNumber;
}