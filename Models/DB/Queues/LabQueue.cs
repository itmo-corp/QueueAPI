using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QueueAPI.Models.DB.Queues;

public class LabQueue
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public List<QueueMember> Members { get; set; } = new List<QueueMember>();
    public List<ObjectId> Maintainers { get; set; } = new List<ObjectId>();
    public string Name { get; set; } = null!;
    public string? Password { get; set; } = null;
}
