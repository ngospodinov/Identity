using Application.Common;
using Application.Exceptions;
using Application.Handlers.Users.Dtos;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.AccessGrants.Get.GetPaged.User;

public class GetPagedAccessGrantsHandler(IAccessGrantRepository accessGrantRepository, IUserRepository userRepository) : IRequestHandler<GetPagedAccessGrantsRequest, PagedResult<AccessGrantDto>>
{
    public async Task<PagedResult<AccessGrantDto>> Handle(GetPagedAccessGrantsRequest request,
        CancellationToken cancellationToken)
    {
        var userExists = await userRepository.ExistsAsync(request.DataOwnerUserId, cancellationToken);
        if (!userExists)
        {
            throw new NotFoundException($"User with id {request.DataOwnerUserId} does not exist");
        }
        
        var accessGrants = await accessGrantRepository.GetAccessGrantsAsync(request.DataOwnerUserId, request.Category, request.PageNumber, request.PageSize, cancellationToken);

        return new PagedResult<AccessGrantDto>
        {
            Items = accessGrants,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = await accessGrantRepository.CountOwnerAsync(request.DataOwnerUserId, cancellationToken)
        };
    }
}