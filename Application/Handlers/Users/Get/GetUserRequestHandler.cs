using Application.Exceptions;
using Application.Handlers.Users.Dtos;
using Application.Repositories;
using Application.Services;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.Users.Get;

public class GetUserRequestHandler(IUserRepository userRepository, ICurrentClientProvider currentClientProvider) : IRequestHandler<GetUserRequest, UserLeanDto>
{
    public async Task<UserLeanDto> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var currentUser = request.RequesterUserId;
        if (string.IsNullOrEmpty(currentUser))
        {
            throw new UnauthorizedAccessException("Missing sub in token.");
        }
        
        var dataOwnerUser = await userRepository.GetUserByIdAsync(request.DataOwnerUserId, cancellationToken);
        if (dataOwnerUser == null)
        {
            throw new NotFoundException($"User with id {request.DataOwnerUserId} not found");
        }
        
        return new UserLeanDto
        {
            Id = dataOwnerUser.Id,
            Username = dataOwnerUser.UserName,
            Email = dataOwnerUser.Email,
        };
    }
}