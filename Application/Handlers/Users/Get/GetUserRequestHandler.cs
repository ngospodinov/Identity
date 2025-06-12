using Application.Handlers.Users.Dtos;
using Application.Repositories;
using Application.Services;
using MediatR;

namespace Application.Handlers.Users.Get;

public class GetUserRequestHandler(IUserRepository userRepository, ICurrentClientProvider currentClientProvider) : IRequestHandler<GetUserRequest, UserDto>
{
    public async Task<UserDto> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var currentClient = currentClientProvider.GetCurrentClientId();
        if (string.IsNullOrEmpty(currentClient))
        {
            throw new UnauthorizedAccessException("Missing client_id in token.");
        }
        
        var user = await userRepository.GetUserByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        var allowedCategories = user.AccessGrants
            .Where(x => x.ClientId == currentClient)
            .Select(x=> x.Category)
            .ToHashSet();
        
        var dataItems = user.DataItems
            .Where(x => allowedCategories.Contains(x.Category))
            .Select(x => new UserDataItemDto
            {
                Key = x.Key,
                Value = x.Value,
                Category = x.Category,
            })
            .ToList();
        
        return new UserDto()
        {
            Id = user.Id,
            Email = user.Email,
            DataItems = dataItems
        };
    }
}