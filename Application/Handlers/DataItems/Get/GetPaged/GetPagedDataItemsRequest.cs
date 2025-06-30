using Application.Common;
using Application.Handlers.Users.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.DataItems.Get.GetPaged;

public class GetPagedDataItemsRequest(string userId, string? categoryFilter, int pageSize = 10, int pageNumber = 1): IRequest<PagedResult<UserDataItemDto>>
{
    public string DataOwnerUserId { get; set; } = userId;

    public DataCategory? Category { get; set; } = categoryFilter?.ParseDataCategory();

    public int PageSize { get; set; } = pageSize;
    
    public int PageNumber { get; set; } = pageNumber;
}