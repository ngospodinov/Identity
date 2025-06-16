namespace Application.Handlers.Users.Dtos;

public class UserDeletedDto
{
    public string Id { get; set; } = null!;
    
    public DateTime? ErasedAtUtc { get; set; }
}