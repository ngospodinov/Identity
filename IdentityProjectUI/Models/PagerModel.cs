namespace IdentityProjectUI.Models;

public class PagerModel
{
    public string BasePath { get; set; } = "/"; 
    
    public int PageNumber { get; set; }
    
    public int PageSize { get; set; }
    
    public int TotalCount { get; set; }
    
    public string? ExtraQuery { get; set; }
    
    public string ItemsLabel { get; set; } = "Items"; 
    
    public int[] PageSizes { get; set; } = [5, 10, 20, 50];
}