using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

public class Name
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;
    
    public User User { get; set; } = null!;

    public DataCategory Category { get; set; }

    public string FirstName { get; set; } = null!;
    
    public string? MiddleName { get; set; }
    
    public string? LastName { get; set; }
    
    public bool IsDefaultForCategory { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;
    
    public DateTime? DeletedAt { get; set; }
    
    [NotMapped]
    public string Display =>
        string.Join(" ", new[] { FirstName, MiddleName, LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
}