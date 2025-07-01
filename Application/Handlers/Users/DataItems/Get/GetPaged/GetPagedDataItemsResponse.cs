using Application.Handlers.Users.Dtos;

namespace Application.Handlers.Users.DataItems.Get.GetPaged;

public class GetPagedDataItemsResponse
{
    public IEnumerable<UserDataItemDto> DataItems { get; set; } = [];

    public int PageNumber { get; set; }
    
    public int PageSize { get; set; }
    
    public int TotalCount { get; set; }
}