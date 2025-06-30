using Application.Handlers.Users.Dtos;
using MediatR;

namespace Application.Handlers.Names.GetById;

public class GetNameRequest( int id, string userId) : IRequest<NameDto>
{
    public string UserId { get; set; } = userId;

    public int Id { get; set; } = id;
}