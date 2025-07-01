using Domain.Enums;
using MediatR;

namespace Application.Handlers.Users.DataItems.Create;

public class CreateDataItemRequest : IRequest<int>
{
    public Guid UserId { get; private set; }
    
    public string Key { get; init; } = null!;
    
    public string Value { get; init; } = null!;
    
    public DataCategory Category { get; init; }

    public void SetId(Guid userId)
    {
        UserId = userId;
    }
}