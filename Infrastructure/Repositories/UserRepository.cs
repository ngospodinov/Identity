using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(IdentityDbContext dbContext) : IUserRepository
{
    public async Task CreateUserAsync(UserEntity user, CancellationToken cancellationToken)
    {
        user.Id = Guid.NewGuid();
        dbContext.Users.Add(user);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserEntity?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .Include(x => x.DataItems)
            .Include(x => x.AccessGrants)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }
}