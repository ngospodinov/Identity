namespace IdentityProjectUI.Models;

public class NameRowModel
{
    public int Id { get; set; } 
    
    public string Category { get; set; } = "Public";
    
    public string FirstName { get; set; } = "";
    
    public string? MiddleName { get; set; }
    
    public string? LastName { get; set; }
    
    public bool IsDefaultForCategory { get; set; }

    public NameDto ToDto()
    {
        return new NameDto
        {
            Category = Category,
            FirstName = FirstName,
            MiddleName = MiddleName,
            LastName = LastName,
            IsDefaultForCategory = IsDefaultForCategory
        };
    }
}