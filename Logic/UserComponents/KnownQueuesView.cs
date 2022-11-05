using QueueAPI.Models.DB.Queues;
using MongoDB.Bson;
using MongoDB.Driver;

namespace QueueAPI.Logic.PlayerComponents;

public class KnownQueuesView
{
    public ObjectId Id { get; init; }

    public KnownQueuesView(ObjectId id)
    {
        Id = id;
    }

    private static IMongoCollection<UserKnownQueues> _collection = null!;
    public static IMongoCollection<UserKnownQueues> Collection => _collection ?? (_collection = Static.DB.GetCollection<UserKnownQueues>("UserKnownQueues"));

    public async Task<UserKnownQueues> Get()
    {
        return await Collection.Find(x => x.Id == Id.ToString()).FirstOrDefaultAsync() ?? new UserKnownQueues();
    }

    public async Task Add(string id)
    {
        await Collection.UpdateOneAsync(x => x.Id == Id.ToString(),
            Builders<UserKnownQueues>.Update.AddToSet(x => x.Items, ObjectId.Parse(id)), new UpdateOptions { IsUpsert = true });
    }

    public async Task Remove(string id)
    {
        await Collection.UpdateOneAsync(x => x.Id == Id.ToString(),
            Builders<UserKnownQueues>.Update.Pull(x => x.Items, ObjectId.Parse(id)), new UpdateOptions { IsUpsert = true });
    }

    public async Task<bool> Contains(string id)
    {
        return await Collection.CountDocumentsAsync(Builders<UserKnownQueues>.Filter.Eq("_id", Id)
            & Builders<UserKnownQueues>.Filter.AnyEq(x => x.Items, ObjectId.Parse(id))) != 0;
    }
}
