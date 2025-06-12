using Application.Handlers.Users.Dtos;
using MediatR;

namespace Application.Handlers.Users.Get;

public class GetUserRequest(Guid userId) : IRequest<UserDto>
{
    public Guid UserId { get; set; } = userId;
}