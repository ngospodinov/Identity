using Application.Common.Models;
using Application.Handlers.Users.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.AccessGrants.Get.GetPaged;

public class GetPagedAccessGrantsRequest(Guid userId,int pageSize = 10, int pageNumber = 1) : IRequest<PagedResult<AccessGrantDto>>
{
    public Guid UserId { get; set; } = userId;
    
    public DataCategory? Category { get; set; } = null;
    
    public int PageSize { get; set; } = pageSize;

    public int PageNumber { get; set; } = pageNumber;
    
}