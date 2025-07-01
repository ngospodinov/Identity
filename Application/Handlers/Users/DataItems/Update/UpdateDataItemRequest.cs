using MediatR;

namespace Application.Handlers.Users.DataItems.Update;

public class UpdateDataItemRequest : IRequest<int>
{
    public Guid UserId { get; set; }
    
    public int DataItemId { get; set; }
    
    public string Value { get; set;  }

    public void SetId(Guid userId, int dataItemId)
    {
        UserId = userId;
        DataItemId = dataItemId;
    }
}