using Application.Handlers.Users.Dtos;
using Application.Repositories;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DataItemRepository(IdentityDbContext dbContext) : IDataItemRepository
{
    public async Task<List<UserDataItemDto>> GetUserDataItemsAsync(Guid userId, DataCategory? category, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = dbContext.UserDataItems.Where(x =>
            x.UserId == userId && (category == null || x.Category == category));
        
       var items =  await query
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .Select(x => new UserDataItemDto
            {
                Key = x.Key,
                Value = x.Value,
                Category = x.Category,
            })
            .ToListAsync(cancellationToken);

       return items;
    }

    public async Task<bool> ExistsAsync(int dataItemId, CancellationToken cancellationToken)
    {
        return await dbContext.UserDataItems.AnyAsync(x => x.Id == dataItemId, cancellationToken);
    }
}