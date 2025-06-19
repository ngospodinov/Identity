using Application.Exceptions;
using Application.Handlers.Requesters.Dtos;
using Application.Repositories;
using MediatR;

namespace Application.Handlers.AccessRequests.Get.GetById;

public class GetAccessRequestByIdHandler(IAccessRequestRepository accessRequestRepository) : IRequestHandler<GetAccessRequestByIdRequest, AccessRequestDto>
{
    public async Task<AccessRequestDto> Handle(GetAccessRequestByIdRequest request, CancellationToken cancellationToken)
    {
        var accessRequestExists = await accessRequestRepository.ExistsAsync(request.RequestId, cancellationToken);

        if (!accessRequestExists)
        {
            throw new NotFoundException($"Access request with id: {request.RequestId} was not found.");
        }
        
        return await accessRequestRepository.GetByIdAsync(request.RequestId, cancellationToken);
    }
}