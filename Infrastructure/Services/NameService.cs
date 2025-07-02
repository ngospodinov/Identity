using Application.Handlers.Users.Dtos;
using Application.Repositories;
using Application.Services;
using Domain.Enums;

namespace Infrastructure.Services;

public sealed class NameService(INameRepository nameRepository, IAccessGrantRepository grantRepository) : INameService
{
    public async Task<List<NameDto>> GetMyNamesAsync(string userId, CancellationToken ct)
    {
        var names = await nameRepository.GetByUserAsync(userId, includeDeleted: false, ct);
            
        return names.Select(x => new NameDto
        {
            Id = x.Id,
            Category = x.Category.ToString(),
            FirstName = x.FirstName,
            MiddleName = x.MiddleName,
            LastName = x.LastName,
            IsDefaultForCategory = x.IsDefaultForCategory
        }).ToList();
    }
    
    public async Task<List<DisplayNameDto>> RetrieveAllowedNamesAsync(
        string requesterId,
        string dataOwnerId,
        CancellationToken ct)
    {
        var names = await nameRepository.GetByUserAsync(dataOwnerId, includeDeleted: false, ct);
        var accessGrants = await grantRepository.GetAllowedCategoriesAsync(requesterId, dataOwnerId, ct);
        accessGrants.Add(DataCategory.Public);

        
        var result = names
            .Where(n => accessGrants.Contains(n.Category))
            .GroupBy(n => n.Category)
            .Select(g => g
                .OrderByDescending(n => n.IsDefaultForCategory)
                .ThenBy(n => n.Id)
                .First())
            .Select(n => new DisplayNameDto(n.Category.ToString(), n.Display))
            .OrderBy(x => x.Category)
            .ToList();

        return result;    
    }
}