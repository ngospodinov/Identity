using Application.Handlers.Users.Dtos;
using MediatR;

namespace Application.Handlers.Users.Get;

public class GetUserRequest(string dataOwnerUserId, string requesterUserId) : IRequest<UserLeanDto>
{
    public string DataOwnerUserId { get; set; } = dataOwnerUserId;
    
    public string RequesterUserId { get; set; } = requesterUserId;
}