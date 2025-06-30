using Application.Handlers.Users.Dtos;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.Names.GetDisplayNames;

public class GetDisplayNamesRequest(string dataOwnerId, string requesterId, IReadOnlyCollection<DataCategory>? categories, bool isAdmin = false) : IRequest<List<DisplayNameDto>>
{
    public string DataOwnerId { get; set; } = dataOwnerId;
    
    public string RequesterId { get; set; } = requesterId;

    public bool IsAdmin { get; set; } = isAdmin;

    private IReadOnlyCollection<DataCategory>? Categories { get; set; } = categories;
}