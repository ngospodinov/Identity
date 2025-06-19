using Application.Common;
using Application.Exceptions;
using Application.Handlers.Requesters.Dtos;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.AccessRequests.Get.GetPaged.Requester;

public class GetPagedAccessGrantsByRequesterUserHandler(
    IUserRepository userRepository,
    IAccessRequestRepository accessRequestRepository)
    : IRequestHandler<GetPagedAccessRequestsByRequesterRequest, PagedResult<AccessRequestDto>>
{
    public async Task<PagedResult<AccessRequestDto>> Handle(GetPagedAccessRequestsByRequesterRequest request,
        CancellationToken cancellationToken)
    {
        var requesterExists = await userRepository.ExistsAsync(request.RequesterUserId, cancellationToken);
        if (!requesterExists)
        {
            throw new NotFoundException($"User with id: {request.RequesterUserId} does not exist.");
        }

        var accessRequests = await accessRequestRepository.GetAccessRequestsAsync(request.RequesterUserId, request.DataOwnerUserId,
            request.Category, request.PageNumber, request.PageSize, cancellationToken);

        return new PagedResult<AccessRequestDto>
        {
            Items = accessRequests,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = await accessRequestRepository.CountRequesterAsync(request.RequesterUserId, cancellationToken)
        };
    }
}