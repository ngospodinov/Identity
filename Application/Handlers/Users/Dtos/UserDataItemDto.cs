namespace Application.Handlers.Users.Dtos;

public class UserDataItemDto
{
    public int Id { get; set; }
    
    public string Key { get; set; } = null!;
    
    public string Value { get; set; } = null!;
    
    public string Category { get; set; }
}