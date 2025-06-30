using Application.Common;
using Application.Handlers.Users.Dtos;
using Application.Repositories;
using Application.Services;
using MediatR;

namespace Application.Handlers.Names.GetMyNames;

public class GetMyNamesHandler(INameService nameService, INameRepository nameRepository) : IRequestHandler<GetMyNamesRequest, PagedResult<NameDto>>
{
    public async Task<PagedResult<NameDto>> Handle(GetMyNamesRequest request, CancellationToken cancellationToken)
    {
        var names = await nameService.GetMyNamesAsync(request.UserId, cancellationToken);
        
        return new PagedResult<NameDto>
        {
            Items = names,
            TotalCount = await nameRepository.CountAsync(request.UserId, cancellationToken),
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
        };
    }
}