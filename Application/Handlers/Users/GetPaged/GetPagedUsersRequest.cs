using Application.Common;
using Application.Handlers.Users.Dtos;
using MediatR;

namespace Application.Handlers.Users.GetPaged;

public class GetPagedUsersRequest(string currentUserId, int pageSize = 1, int pageNumber = 10, string? search = null) : IRequest<PagedResult<UserLeanDto>>
{
    public string CurrentUserId { get; set; } = currentUserId;
    
    public int PageSize { get; set; } = pageSize;

    public int PageNumber { get; set; } = pageNumber;

    public string? Search { get; set; } = search;
}