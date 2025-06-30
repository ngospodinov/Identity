using Domain.Enums;
using MediatR;

namespace Application.Handlers.DataItems.Update;

public class UpdateDataItemRequest : IRequest<int>
{
    public string DataOwnerUserId { get; set; } = null!;
    
    public int DataItemId { get; set; }
    
    public string Key { get; set; } = null!;
    
    public string Value { get; set;  } = null!;
    
    public DataCategory Category { get; set; }
    
}