using Application.Common.Models;
using Application.Exceptions;
using Application.Handlers.Institutions.Dtos;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.AccessRequests.Get.GetPaged;

public class GetPagedAccessRequestHandler(IAccessRequestRepository accessRequestRepository, IUserRepository userRepository) : IRequestHandler<GetPagedAccessRequest, PagedResult<AccessRequestDto>>
{
    public async Task<PagedResult<AccessRequestDto>> Handle(GetPagedAccessRequest request,
        CancellationToken cancellationToken)
    {
        var userExists = await userRepository.ExistsAsync(request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new NotFoundException($"User with id {request.UserId} not found.");
        }
        
        var accessRequests = await accessRequestRepository.GetAccessRequestsAsync(request.UserId, request.PageSize, request.PageNumber, request.InstitutionId, cancellationToken);

        return new PagedResult<AccessRequestDto>()
        {
            Items = accessRequests,
            PageSize = request.PageSize,
            PageNumber = request.PageNumber,
            TotalCount = accessRequests.Count
        };
    }
}