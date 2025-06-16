namespace Application.Handlers.Users.Dtos;

public class NameDto
{
    public int Id { get; set; }
    
    public string Category { get; set; }

    public string FirstName { get; set; } = null!;
    
    public string? MiddleName { get; set; }
    
    public string? LastName { get; set; }
    
    public bool IsDefaultForCategory { get; set; }
}