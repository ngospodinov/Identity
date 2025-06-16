using Application.Handlers.Users.Dtos;
using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .Where(x => x.DeletedAt == null)
            .CountAsync(cancellationToken);
    }
    
    public async Task<List<UserLeanDto>> GetUsersAsync(string currentUserId, int pageNumber, int pageSize, string? search, CancellationToken ct)
    {
        var query = dbContext.Users
            .AsNoTracking()
            .Where(user => !user.IsDeleted && user.Id != currentUserId);
        
        
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();

            if (Guid.TryParse(s, out _))
            {
                query = query.Where(user => user.Id == s);
            }
            else
            {
                query = query.Where(user =>
                    EF.Functions.ILike(user.UserName ?? "", $"%{s}%") ||
                    EF.Functions.ILike(user.Email    ?? "", $"%{s}%"));
            }
        }
        
        return await query
            .OrderBy(user => user.UserName)   
            .ThenBy(user => user.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new UserLeanDto
            {
                Id = x.Id,
                Username = x.UserName,
                Email = x.Email,
            })
            .ToListAsync(ct);
    }
    
    public async Task<List<UserLeanDto>> GetGrantedUsersAsync(string currentUserId, int pageNumber, int pageSize,
        string? search, CancellationToken ct)
    {
        var query = dbContext.Users
            .AsNoTracking()
            .Where(user => !user.IsDeleted 
                           && user.Id != currentUserId
                           && dbContext.AccessGrants.Any(g =>
                               g.DataOwnerUserId == user.Id &&
                               g.RequesterUserId == currentUserId &&
                               g.RevokedAt == null));
        
        
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();

            if (Guid.TryParse(s, out _))
            {
                query = query.Where(user => user.Id == s);
            }
            else
            {
                query = query.Where(user =>
                    EF.Functions.ILike(user.UserName ?? "", $"%{s}%") ||
                    EF.Functions.ILike(user.Email    ?? "", $"%{s}%"));
            }
        }
        
        return await query
            .OrderBy(user => user.UserName)   
            .ThenBy(user => user.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new UserLeanDto
            {
                Id = x.Id,
                Username = x.UserName,
                Email = x.Email,
            })
            .ToListAsync(ct);
    }
    
    public async Task CreateUserAsync(User user, CancellationToken ct)
    {
        user.Id = Guid.NewGuid().ToString();
        dbContext.Users.Add(user);
        
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task<User?> GetUserByIdAsync(string userId, CancellationToken ct)
    {
        return await dbContext.Users
            .Include(x => x.DataItems)
            .FirstOrDefaultAsync(x => x.Id == userId, ct);
    }

    public async Task<bool> ExistsAsync(string userId, CancellationToken ct)
    {
        return await dbContext.Users.AnyAsync(x => x.Id == userId, ct);
    }
}