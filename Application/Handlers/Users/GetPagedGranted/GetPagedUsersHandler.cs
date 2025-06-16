using Application.Common;
using Application.Handlers.Users.Dtos;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.Users.GetPagedGranted;

public class GetPagedUsersHandler(IUserRepository userRepository) : IRequestHandler<GetPagedGrantedUsersRequest, PagedResult<UserLeanDto>>
{
    public async Task<PagedResult<UserLeanDto>> Handle(GetPagedGrantedUsersRequest request,
        CancellationToken cancellationToken)
    {
        var users = await userRepository.GetGrantedUsersAsync(request.CurrentUserId, request.PageNumber, request.PageSize, request.Search, cancellationToken);
        
        return new PagedResult<UserLeanDto>
        {
            PageSize = request.PageSize,
            PageNumber = request.PageNumber,
            Items = users,
            TotalCount = users.Count
        };
    }
}