using Application.Common;
using Application.Exceptions;
using Application.Handlers.Requesters.Dtos;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.AccessRequests.Get.GetPaged.User;

public class GetPagedAccessRequestHandler(IAccessRequestRepository accessRequestRepository, IUserRepository userRepository) : IRequestHandler<GetPagedAccessRequest, PagedResult<AccessRequestDto>>
{
    public async Task<PagedResult<AccessRequestDto>> Handle(GetPagedAccessRequest request,
        CancellationToken cancellationToken)
    {
        var userExists = await userRepository.ExistsAsync(request.DataOwnerUserId, cancellationToken);
        if (!userExists)
        {
            throw new NotFoundException($"User with id {request.DataOwnerUserId} not found.");
        }
        
        var accessRequests = await accessRequestRepository.GetAccessRequestsAsync(request.DataOwnerUserId, request.PageNumber, request.PageSize ,cancellationToken);

        return new PagedResult<AccessRequestDto>()
        {
            Items = accessRequests,
            PageSize = request.PageSize,
            PageNumber = request.PageNumber,
            TotalCount = await accessRequestRepository.CountOwnerAsync(request.DataOwnerUserId, cancellationToken)
        };
    }
}