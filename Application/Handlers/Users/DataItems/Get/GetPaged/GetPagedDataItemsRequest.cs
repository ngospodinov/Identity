using Application.Common.Models;
using Application.Handlers.Users.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.Users.DataItems.Get.GetPaged;

public class GetPagedDataItemsRequest(Guid userId, int pageSize = 10, int pageNumber = 1): IRequest<PagedResult<UserDataItemDto>>
{
    public Guid UserId { get; set; } = userId;
    
    public DataCategory? Category { get; set; }

    public int PageSize { get; set; } = pageSize;
    
    public int PageNumber { get; set; } = pageNumber;
}