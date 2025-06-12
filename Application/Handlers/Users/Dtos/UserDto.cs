
namespace Application.Handlers.Users.Dtos;

public class UserDto
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;
    
    public ICollection<UserDataItemDto> DataItems { get; set; } = [];
}