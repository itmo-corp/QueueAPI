using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QueueAPI.Models.DB.Queues;

public class QueueMember
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;
    public bool IsReady { get; set; } = false;
}
