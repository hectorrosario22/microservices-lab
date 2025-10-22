using MongoDB.Driver;

namespace Auth.Api.Data;

public interface IMongoDatabaseConnection
{
    IMongoDatabase? Database { get; }
}
