using System.ComponentModel.DataAnnotations;
using Application.Common;
using Application.Repositories;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.Names.Create;

public class CreateNameHandler(INameRepository nameRepository) : IRequestHandler<CreateNameRequest>
{
    public async Task Handle(CreateNameRequest request, CancellationToken cancellationToken)
    {
        var nameExists = await nameRepository.ExistsAsync(request.UserId,  request.FirstName, request.Category.ParseDataCategory(), request.MiddleName, request.LastName, cancellationToken);
        if (nameExists)
        {
            return;
        }

        var nameEntity = new Name
        {
            UserId = request.UserId,
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            Category = request.Category.ParseDataCategory(),
            IsDefaultForCategory = request.IsDefaultForCategory,
        };
        
        if (request.IsDefaultForCategory)
        {
            await nameRepository.UnsetDefaultsAsync(request.UserId, [request.Category.ParseDataCategory()], cancellationToken);
        }
        
        await nameRepository.CreateAsync(nameEntity, cancellationToken);
    }
}