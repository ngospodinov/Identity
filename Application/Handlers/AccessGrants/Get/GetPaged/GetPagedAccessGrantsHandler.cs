using Application.Common.Models;
using Application.Exceptions;
using Application.Handlers.Users.Dtos;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.AccessGrants.Get.GetPaged;

public class GetPagedAccessGrantsHandler(IAccessGrantRepository accessGrantRepository, IUserRepository userRepository) : IRequestHandler<GetPagedAccessGrantsRequest, PagedResult<AccessGrantDto>>
{
    public async Task<PagedResult<AccessGrantDto>> Handle(GetPagedAccessGrantsRequest request,
        CancellationToken cancellationToken)
    {
        var userExists = await userRepository.ExistsAsync(request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new NotFoundException($"User with id {request.UserId} does not exist");
        }
        
        var accessGrants = await accessGrantRepository.GetAccessGrantsAsync(request.UserId, request.Category, request.PageNumber, request.PageSize, cancellationToken);

        return new PagedResult<AccessGrantDto>
        {
            Items = accessGrants,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = accessGrants.Count
        };
    }
}