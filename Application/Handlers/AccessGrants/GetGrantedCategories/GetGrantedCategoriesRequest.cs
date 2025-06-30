using Application.Handlers.Users.Dtos;
using MediatR;

namespace Application.Handlers.AccessGrants.GetGrantedCategories;

public class GetGrantedCategoriesRequest(string loggedUserId, string dataOwnerUserId) : IRequest<List<GrantedCategoryDto>>
{
    public string LoggedUserId { get; set; } = loggedUserId;
    
    public string DataOwnerUserId { get; set; } = dataOwnerUserId;
}