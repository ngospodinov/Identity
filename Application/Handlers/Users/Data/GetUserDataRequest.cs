using Application.Handlers.Users.Dtos;
using MediatR;

namespace Application.Handlers.Users.Data;

public class GetUserDataRequest(string dataOwnerUserId, string requesterUserId,
    string category, int pageSize = 10, int pageNumber = 1) : IRequest<List<UserDataItemDto>>
{
    public string DataOwnerUserId { get; set; } = dataOwnerUserId;
    
    public string RequesterUserId { get; set; } = requesterUserId;

    public string Category { get; set; } = category;
}