using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QueueAPI.Models.API.Queues;

public class QueueMemberResponce
{
    public string DisplayName { get; set; } = null!;
    public bool IsReady { get; set; }
}
