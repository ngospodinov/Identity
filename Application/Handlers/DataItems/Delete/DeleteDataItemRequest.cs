using MediatR;

namespace Application.Handlers.DataItems.Delete;

public class DeleteDataItemRequest(string userId, int dataItemId) : IRequest
{
    public string DataOwnerUserId { get; set; } = userId;
    
    public int DataItemId { get; set; } = dataItemId;
}