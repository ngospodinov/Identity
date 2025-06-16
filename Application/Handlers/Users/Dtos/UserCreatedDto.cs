namespace Application.Handlers.Users.Dtos;

public class UserCreatedDto
{
    public string Id { get; set; } = null!;
    
    public string? UserName { get; set; } = null!;
    
    public string? Email { get; set; } = null!;
}