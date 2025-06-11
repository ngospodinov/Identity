using Domain.Entities;

namespace Application.Repositories;

public interface IUserRepository
{
    Task CreateUserAsync(UserEntity user);
}