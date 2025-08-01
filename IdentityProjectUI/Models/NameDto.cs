namespace IdentityProjectUI.Models;

public class NameDto
{
    public int Id { get; set; }

    public string Category { get; set; } = string.Empty;

    public string FirstName { get; set; } = null!;
    
    public string? MiddleName { get; set; }
    
    public string? LastName { get; set; }
    
    public bool IsDefaultForCategory { get; set; }
    
    public string FullName =>
        string.Join(" ", new[] { FirstName, MiddleName, LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
}