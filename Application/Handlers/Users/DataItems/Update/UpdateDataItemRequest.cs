using MediatR;

namespace Application.Handlers.Users.DataItems.Update;

public class UpdateDataItemRequest : IRequest<int>
{
    public Guid UserId { get; private set; }
    
    public int DataItemId { get; private set; }
    
    public string Value { get; set;  }

    public void SetId(Guid userId, int dataItemId)
    {
        UserId = userId;
        DataItemId = dataItemId;
    }
}