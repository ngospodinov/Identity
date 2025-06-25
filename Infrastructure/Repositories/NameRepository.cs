using Application.Handlers.Users.Dtos;
using Application.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NameRepository(ApplicationDbContext dbContext) : INameRepository
{
    public async Task<int> CountAsync(string? userId, CancellationToken cancellationToken)
    {
        return await dbContext.Names
            .Where(x => x.DeletedAt == null)
            .CountAsync(x => userId == null || x.UserId == userId, cancellationToken);
    }
    
    public Task<List<Name>> GetByUserAsync(string userId, bool includeDeleted, CancellationToken ct) =>
        dbContext.Names
            .Where(n => n.UserId == userId && (includeDeleted || !n.IsDeleted))
            .OrderBy(n => n.Category).ThenBy(n => n.Id)
            .ToListAsync(ct);

    public Task<List<Name>> GetForCategoryAsync(string userId, DataCategory category, bool includeDeleted,
        CancellationToken ct) =>
        dbContext.Names
            .Where(n => n.UserId == userId && n.Category == category && (includeDeleted || !n.IsDeleted))
            .OrderBy(n => n.Id)
            .ToListAsync(ct);

    public Task<Name?> GetByIdAsync(int id, CancellationToken ct) =>
        dbContext.Names.FirstOrDefaultAsync(n => n.Id == id, ct);
    
    public Task<Name?> GetByIdAsync(int id, string userId, CancellationToken ct) =>
        dbContext.Names.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId, ct);

    public async Task<bool> ExistsAsync(string dataOwnerUserId, string firstName, DataCategory category, string? middleName, string? lastName,
        CancellationToken ct)
    {
        return await dbContext.Names.AnyAsync(x => x.UserId == dataOwnerUserId && 
                                                   firstName == x.FirstName && 
                                                   category == x.Category &&
                                                   middleName == x.MiddleName &&
                                                   lastName == x.LastName, ct);
    }

    public async Task CreateAsync(Name name, CancellationToken ct)
    {
        dbContext.Names.Add(name);
        await dbContext.SaveChangesAsync(ct);
    }

    
    public async Task UnsetDefaultsAsync(string userId, IEnumerable<DataCategory> categories, CancellationToken ct)
    {
        var set = await dbContext.Names
            .Where(n => n.UserId == userId && !n.IsDeleted && categories.Contains(n.Category))
            .ToListAsync(ct);

        foreach (var n in set) n.IsDefaultForCategory = false;
        
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task SoftDeleteAsync(int id, CancellationToken ct)
    {
        var n = await dbContext.Names.FirstOrDefaultAsync(x => x.Id == id, ct)
                ?? throw new KeyNotFoundException($"Name {id} not found.");
        n.IsDeleted = true;
        n.DeletedAt = DateTime.UtcNow;
    }

    public Task<bool> HasDefaultAsync(string userId, DataCategory category, CancellationToken ct) =>
        dbContext.Names.AnyAsync(
            n => n.UserId == userId && n.Category == category && !n.IsDeleted && n.IsDefaultForCategory, ct);

    public Task<bool> HasDefaultPublicProfileAsync(string userId, CancellationToken ct) =>
        HasDefaultAsync(userId, DataCategory.Public, ct);

    public Task SaveChangesAsync(CancellationToken ct) => dbContext.SaveChangesAsync(ct);
}