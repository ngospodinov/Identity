using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class UserRepository(IdentityDbContext dbContext) : IUserRepository
{
    public async Task CreateUserAsync(UserEntity user)
    {
        user.Id = Guid.NewGuid();
        dbContext.Users.Add(user);
        
        await dbContext.SaveChangesAsync();
    }
}