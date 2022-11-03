using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QueueAPI.Models.DB.Auth;

public class APIToken
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
}
