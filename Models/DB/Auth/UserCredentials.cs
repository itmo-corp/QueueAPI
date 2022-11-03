using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QueueAPI.Models.DB.Auth;

public class UserCredentials
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string TelegramId { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
}
