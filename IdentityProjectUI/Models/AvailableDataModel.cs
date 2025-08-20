namespace IdentityProjectUI.Models;

public class AvailableDataModel
{
    public UserDto UserData { get; set; } = new UserDto();
    
    public NameDto NameData { get; set; } = new NameDto(); 
    
    public List<DataItemDto> DataItems { get; set; } = [];
    
    public string Category { get; set; } = string.Empty;
}