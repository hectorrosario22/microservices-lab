using Auth.Api.Data;
using Auth.Api.Entities;

namespace Auth.Api.Repositories;

public class UserRepository(IMongoDatabaseConnection connection) :IUserRepository
{
    public async Task CreateUserAsync(User user)
    {
        var usersCollection = connection.Database?.GetCollection<User>("users");
        if (usersCollection != null)
        {
            await usersCollection.InsertOneAsync(user);
        }
    }
}