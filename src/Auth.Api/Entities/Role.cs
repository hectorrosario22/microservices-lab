using MongoDB.Bson.Serialization.Attributes;

namespace Auth.Api.Entities;

public class Role
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    
    [BsonElement("name")]
    public required string Name { get; set; }
}