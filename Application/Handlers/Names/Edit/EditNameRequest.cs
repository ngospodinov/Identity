using MediatR;

namespace Application.Handlers.Names.Edit;

public class EditNameRequest() : IRequest
{
    public string UserId { get; set; } 
    
    public int Id { get; set; }
    
    public string Category { get; set; } = "";       
    
    public string FirstName { get; set; } = "";
    
    public string? MiddleName { get; set; }
    
    public string? LastName { get; set; }
    
    public bool IsDefaultForCategory { get; set; }
}