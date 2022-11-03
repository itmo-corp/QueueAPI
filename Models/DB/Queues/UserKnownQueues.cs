using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QueueAPI.Models.DB.Queues;

public class UserKnownQueues
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public List<ObjectId> Items = new List<ObjectId>();
}
