using Application.Common;
using Application.Handlers.Users.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.Names.GetNameByUser;

public class GetNameByUserRequest(string userId, string category) : IRequest<NameDto>
{
    public string UserId { get; init; } = userId;

    public DataCategory Category { get; init; } = category.ParseDataCategory();
}