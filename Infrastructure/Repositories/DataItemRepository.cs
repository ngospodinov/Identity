using Application.Handlers.Users.Dtos;
using Application.Repositories;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DataItemRepository(ApplicationDbContext dbContext) : IDataItemRepository
{
    public async Task<List<UserDataItemDto>> GetUserDataItemsAsync(string userId, DataCategory? category, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = dbContext.UserDataItems.Where(x =>
            x.UserId == userId && (category == null || x.Category == category) && x.DeletedAt == null);
        
       var items =  await query
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .Select(x => new UserDataItemDto
            {
                Id = x.Id,
                Key = x.Key,
                Value = x.Value,
                Category = x.Category.ParseCategory(),
            })
            .ToListAsync(cancellationToken);

       return items;
    }

    public async Task<int> CountAsync(string? userId, CancellationToken cancellationToken)
    {
        return await dbContext.UserDataItems
            .Where(x => x.DeletedAt == null)
            .CountAsync(x => userId == null || x.UserId == userId, cancellationToken);
    }


    public async Task<List<UserDataItemDto>> GetUserDataItemsForCategoryAsync(string userId, DataCategory category,
        CancellationToken cancellationToken)
    {
        return await dbContext.UserDataItems.Where(x =>
            x.UserId == userId && x.Category == category && x.DeletedAt == null)
            .Select(x => new UserDataItemDto
            {
                Id = x.Id,
                Key = x.Key,
                Value = x.Value,
                Category = x.Category.ParseCategory(),
            })
            .ToListAsync(cancellationToken);
    }
    
    public async Task<UserDataItemDto?> GetDataItemByIdAsync(string userId, int dataItemId,
        CancellationToken cancellationToken)
    {
        return await dbContext.UserDataItems.Where(x => x.Id == dataItemId && x.UserId == userId)
            .Select(x => new UserDataItemDto
            {
                Id = x.Id,
                Key = x.Key,
                Value = x.Value,
                Category = x.Category.ParseCategory(),
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int dataItemId, CancellationToken cancellationToken)
    {
        return await dbContext.UserDataItems.AnyAsync(x => x.Id == dataItemId, cancellationToken);
    }

    public async Task<DataCategory?> GetCategoryAsync(int dataItemId, CancellationToken cancellationToken)
    {
        return await dbContext.UserDataItems
            .Where(x => x.Id == dataItemId)
            .Select(x => x.Category)
            .FirstOrDefaultAsync(cancellationToken);
    }
}