using MongoDB.Driver;

namespace Auth.Api.Data;

internal sealed class MongoDatabaseConnection
{
    private readonly IMongoDatabase _database;

    public MongoDatabaseConnection(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("MongoDB connection string is not configured");

        var mongoUrl = MongoUrl.Create(connectionString);
        var mongoClient = new MongoClient(mongoUrl);
        _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
    }

    public IMongoDatabase? Database => _database;
}