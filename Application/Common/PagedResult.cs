namespace Application.Common;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    
    public int PageSize { get; set; }
    
    public int PageNumber { get; set; }
    
    public int TotalCount { get; set; }
    
    private int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    
    public bool HasPrevious => PageNumber > 1;
    
    public bool HasNext => PageNumber < TotalPages;
    
}