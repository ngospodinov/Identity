using Application.Handlers.Institutions.Dtos;
using MediatR;

namespace Application.Handlers.AccessRequests.Get.GetById;

public class GetAccessRequestByIdRequest(Guid userId, int requestId) : IRequest<AccessRequestDto>
{
    public Guid UserId { get; set; } = userId;

    public int RequestId { get; set; } = requestId;
}