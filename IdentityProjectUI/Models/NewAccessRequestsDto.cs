namespace IdentityProjectUI.Models;

public class NewAccessRequestsDto
{
    public string TargetUserId { get; set; } = null!;
    
    public List<string> SelectedCategories { get; set; } = [];
}