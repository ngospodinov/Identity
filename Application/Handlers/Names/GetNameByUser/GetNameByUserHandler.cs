using Application.Exceptions;
using Application.Handlers.Users.Dtos;
using Application.Repositories;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.Names.GetNameByUser;

public class GetNameByUserHandler(INameRepository nameRepository, IUserRepository userRepository) : IRequestHandler<GetNameByUserRequest, NameDto>
{
    public async Task<NameDto> Handle(GetNameByUserRequest request, CancellationToken cancellationToken)
    {
        var userExists = await userRepository.ExistsAsync(request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new NotFoundException($"User with id {request.UserId} not found");
        }
        
        var names = await nameRepository.GetForCategoryAsync(request.UserId, request.Category, false, cancellationToken);
        
        return names
            .Where(x => x.IsDefaultForCategory)
            .Select(x => new NameDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.LastName,
                Category = x.Category.ParseCategory()
            })
            .FirstOrDefault() ?? new NameDto();
    }
} 
    
