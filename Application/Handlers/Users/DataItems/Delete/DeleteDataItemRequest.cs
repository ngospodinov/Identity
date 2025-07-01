using MediatR;

namespace Application.Handlers.Users.DataItems.Delete;

public class DeleteDataItemRequest(Guid userId, int dataItemId) : IRequest
{
    public Guid UserId { get; set; } = userId;
    
    public int DataItemId { get; set; } = dataItemId;
}