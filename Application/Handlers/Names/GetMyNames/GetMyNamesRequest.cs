using Application.Common;
using Application.Handlers.Users.Dtos;
using MediatR;

namespace Application.Handlers.Names.GetMyNames;

public class GetMyNamesRequest(string userId, int pageSize, int pageNumber) : IRequest<PagedResult<NameDto>>
{
    public string UserId { get; set; } = userId;
    
    public int PageSize { get; set; } = pageSize;
    
    public int PageNumber { get; set; } = pageNumber;
}