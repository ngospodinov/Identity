using Domain.Enums;
using MediatR;

namespace Application.Handlers.Users.DataItems.Get.GetPaged;

public class GetPagedDataItemsRequest(Guid userId): IRequest<GetPagedDataItemsResponse>
{
    public Guid UserId { get; set; } = userId;
    
    public DataCategory? Category { get; set; }

    public int PageSize { get; set; } = 10;

    public int PageNumber { get; set; } = 1;
}