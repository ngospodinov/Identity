using Application.Common;
using Application.Exceptions;
using Application.Handlers.Users.Dtos;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.AccessGrants.Get.GetPaged.Requester;

public class GetPagedAccessGrantsByInstitutionHandler(IUserRepository userRepository, IAccessGrantRepository accessGrantRepository) : IRequestHandler<GetPagedAccessGrantsByRequesterRequest, PagedResult<AccessGrantDto>>
{
    public async Task<PagedResult<AccessGrantDto>> Handle(GetPagedAccessGrantsByRequesterRequest request,
        CancellationToken cancellationToken)
    {
        var requesterExists = await userRepository.ExistsAsync(request.RequesterUserId, cancellationToken);
        if (!requesterExists)
        {
            throw new NotFoundException($"Requester with id: {request.RequesterUserId} does not exist.");
        }
        
        var accessGrants = await accessGrantRepository.GetAccessGrantsAsync(request.RequesterUserId, request.DataOwnerUserId, request.Category, request.PageNumber, request.PageSize, cancellationToken);

        return new PagedResult<AccessGrantDto>
        {
            Items = accessGrants,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = await accessGrantRepository.CountRequesterAsync(request.RequesterUserId, cancellationToken)
        };
    }
}