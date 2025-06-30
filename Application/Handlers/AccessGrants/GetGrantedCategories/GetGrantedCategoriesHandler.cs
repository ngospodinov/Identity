using Application.Exceptions;
using Application.Handlers.Users.Dtos;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.AccessGrants.GetGrantedCategories;

public class GetGrantedCategoriesHandler(IAccessGrantRepository accessGrantRepository, IUserRepository userRepository) : IRequestHandler<GetGrantedCategoriesRequest, List<GrantedCategoryDto>>
{
    public async Task<List<GrantedCategoryDto>> Handle(GetGrantedCategoriesRequest request,
        CancellationToken cancellationToken)
    {
        var exists = await userRepository.ExistsAsync(request.DataOwnerUserId, cancellationToken);
        if (!exists)
        {
            throw new NotFoundException($"User with id {request.DataOwnerUserId} not found");
        }
        
        var accessGrants = await accessGrantRepository.GetAccessGrantsAsync(request.DataOwnerUserId, request.LoggedUserId,  cancellationToken);

        return accessGrants.Select(x => new GrantedCategoryDto
        {
            Id = x.Id,
            CategoryName = x.Category,
            GrantedAt = x.GrantedAt,
        }).ToList();
    }
}