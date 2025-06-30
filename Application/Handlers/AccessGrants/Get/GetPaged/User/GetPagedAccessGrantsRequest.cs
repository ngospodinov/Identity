using Application.Common;
using Application.Handlers.Users.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.AccessGrants.Get.GetPaged.User;

public class GetPagedAccessGrantsRequest(string dataOwnerUserId, DataCategory? category,  int pageSize = 10, int pageNumber = 1) : IRequest<PagedResult<AccessGrantDto>>
{
    public string DataOwnerUserId { get; set; } = dataOwnerUserId;
    
    public DataCategory? Category { get; set; } = category;
    
    public int PageSize { get; set; } = pageSize;

    public int PageNumber { get; set; } = pageNumber;
    
}