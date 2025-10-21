using Auth.Api.Entities;

namespace Auth.Api.Repositories;

public interface IUserRepository
{
    Task CreateUserAsync(User user);
}
