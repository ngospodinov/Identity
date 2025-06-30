using Application.Handlers.Users.Dtos;
using Application.Services;
using MediatR;

namespace Application.Handlers.Names.GetDisplayNames;

public class GetDisplayNamesHandler(INameService nameService) : IRequestHandler<GetDisplayNamesRequest, List<DisplayNameDto>>
{
    public async Task<List<DisplayNameDto>> Handle(GetDisplayNamesRequest request, CancellationToken cancellationToken)
    {
        return await nameService.RetrieveAllowedNamesAsync(request.RequesterId, request.DataOwnerId, cancellationToken);
    }
}