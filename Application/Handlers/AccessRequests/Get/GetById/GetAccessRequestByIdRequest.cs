using Application.Handlers.Requesters.Dtos;
using MediatR;

namespace Application.Handlers.AccessRequests.Get.GetById;

public class GetAccessRequestByIdRequest(string dataOwnerUserId, int requestId) : IRequest<AccessRequestDto>
{
    public string DataOwnerUserId { get; set; } = dataOwnerUserId;

    public int RequestId { get; set; } = requestId;
}