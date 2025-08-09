namespace IdentityProjectUI.Models;

public class NewAccessModel
{
    public string TargetUserId { get; set; } = "";

    public List<string> Categories { get; set; } = ["Public", "Financial", "Academic", "Legal"];
    
    public List<string> SelectedCategories { get; set; } = [];
    
    public PagedResult<DataItemDto> DataItems { get; set; } 
}



