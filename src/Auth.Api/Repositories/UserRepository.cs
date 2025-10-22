using Auth.Api.Data;
using Auth.Api.Entities;
using MongoDB.Driver;

namespace Auth.Api.Repositories;

public class UserRepository(IMongoDatabaseConnection connection) : IUserRepository
{
    private readonly IMongoCollection<User>? _usersCollection = connection.Database?.GetCollection<User>("users");

    public async Task CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        if (_usersCollection is null) return;
        await _usersCollection.InsertOneAsync(user, cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        if (_usersCollection is null) return false;

        return await _usersCollection.Find(u => u.Username == username)
            .AnyAsync(cancellationToken: cancellationToken);
    }
}