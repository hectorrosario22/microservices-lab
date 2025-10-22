using MongoDB.Bson.Serialization.Attributes;

namespace Auth.Api.Entities;

public class User
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("username")]
    public required string Username { get; set; }

    [BsonElement("password_hash")]
    public required string PasswordHash { get; set; }

    [BsonElement("first_name")]
    public required string FirstName { get; set; }

    [BsonElement("last_name")]
    public required string LastName { get; set; }

    [BsonElement("email")]
    public required string Email { get; set; }

    [BsonElement("role_ids")]
    public HashSet<string> RoleIds { get; set; } = [];
}
