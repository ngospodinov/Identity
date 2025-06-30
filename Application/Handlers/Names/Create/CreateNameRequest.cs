using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Application.Handlers.Users.Dtos;
using MediatR;

namespace Application.Handlers.Names.Create;

public class CreateNameRequest() : IRequest
{
    [JsonIgnore]
    public string UserId { get; set; } 
    
    [Required]
    public string Category { get; set; } = "";       
    
    [Required]
    public string FirstName { get; set; } = "";
    
    public string? MiddleName { get; set; }
    
    public string? LastName { get; set; }
    
    public bool IsDefaultForCategory { get; set; }
}
    
