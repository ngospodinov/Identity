namespace Application.Handlers.Users.Dtos;

public class NewAccessRequestsDto
{
    public string TargetUserId { get; set; } = string.Empty;

    public List<string> SelectedCategories { get; set; } = [];
}