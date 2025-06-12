using Domain.Enums;

namespace Application.Handlers.Users.Dtos;

public class UserDataItemDto
{
    public string Key { get; set; } = null!;
    
    public string Value { get; set; } = null!;
    
    public DataCategory Category { get; set; }
}