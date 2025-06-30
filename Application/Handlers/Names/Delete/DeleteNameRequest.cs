using MediatR;

namespace Application.Handlers.Names.Delete;

public class DeleteNameRequest(int id, string userId) : IRequest
{
    public int Id { get; init; } = id;

    public string UserId { get; init; } = userId;
}