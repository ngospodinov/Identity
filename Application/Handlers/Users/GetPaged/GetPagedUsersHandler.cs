using Application.Common;
using Application.Handlers.Users.Dtos;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.Users.GetPaged;

public class GetPagedUsersHandler(IUserRepository userRepository) : IRequestHandler<GetPagedUsersRequest, PagedResult<UserLeanDto>>
{
    public async Task<PagedResult<UserLeanDto>> Handle(GetPagedUsersRequest request,
        CancellationToken cancellationToken)
    {
        var users = await userRepository.GetUsersAsync(request.CurrentUserId, request.PageNumber, request.PageSize, request.Search, cancellationToken);
        
        return new PagedResult<UserLeanDto>
        {
            PageSize = request.PageSize,
            PageNumber = request.PageNumber,
            Items = users,
            TotalCount = await userRepository.CountAsync(cancellationToken)
        };
    }
}