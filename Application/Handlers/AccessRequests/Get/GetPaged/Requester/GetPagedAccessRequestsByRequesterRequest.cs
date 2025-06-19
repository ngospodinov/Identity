using Application.Common;
using Application.Handlers.Requesters.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.AccessRequests.Get.GetPaged.Requester;

public class GetPagedAccessRequestsByRequesterRequest(
    string requesterUserId,
    string? dataOwnerUserId,
    DataCategory? category,
    int pageSize = 10,
    int pageNumber = 1) : IRequest<PagedResult<AccessRequestDto>>
{
    public string RequesterUserId { get; set; } = requesterUserId;

    public string? DataOwnerUserId { get; set; } = dataOwnerUserId;

    public DataCategory? Category { get; set; } = category;

    public int PageSize { get; set; } = pageSize;

    public int PageNumber { get; set; } = pageNumber;
}