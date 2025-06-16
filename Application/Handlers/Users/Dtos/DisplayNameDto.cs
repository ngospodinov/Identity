namespace Application.Handlers.Users.Dtos;

public class DisplayNameDto(string category, string displayName)
{
    public string Category { get; set; } = category;
    
    public string Name { get; set; }  = displayName;
}