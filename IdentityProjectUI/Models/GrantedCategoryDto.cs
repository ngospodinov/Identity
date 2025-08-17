namespace IdentityProjectUI.Models;

public class GrantedCategoryDto
{
    public int Id { get; set; }
    
    public string CategoryName { get; set; } = null!;
    
    public DateTime GrantedAt { get; set; }
}