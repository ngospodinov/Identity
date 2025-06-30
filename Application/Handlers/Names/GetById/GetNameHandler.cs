using Application.Exceptions;
using Application.Handlers.Users.Dtos;
using Application.Repositories;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.Names.GetById;

public class GetNameHandler(INameRepository nameRepository) : IRequestHandler<GetNameRequest, NameDto>
{
    public async Task<NameDto> Handle(GetNameRequest request, CancellationToken cancellationToken)
    {
        var name = await nameRepository.GetByIdAsync(request.Id, request.UserId, cancellationToken);
        if (name == null)
        {
            throw new NotFoundException($"Name with id {request.Id} for user with id {request.UserId} not found");
        }

        return new NameDto
        {
            Id = name.Id,
            FirstName = name.FirstName,
            MiddleName = name.MiddleName,
            LastName = name.LastName,
            Category = name.Category.ParseCategory(),
            IsDefaultForCategory = name.IsDefaultForCategory,
        };
    }
}