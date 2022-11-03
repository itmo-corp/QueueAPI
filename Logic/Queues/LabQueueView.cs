using MongoDB.Bson;
using MongoDB.Driver;
using QueueAPI.Models.DB.Queues;

namespace QueueAPI.Logic.Queues;

public class LabQueueView
{
    private static IMongoCollection<LabQueue> _collection = null!;
    public static IMongoCollection<LabQueue> Collection => _collection ?? (_collection = Static.DB.GetCollection<LabQueue>("Queues"));

    public ObjectId Id { get; init; }

    public LabQueueView(string id) : this(ObjectId.Parse(id)) { }
    public LabQueueView(ObjectId id) { Id = id; }

    // creates empty queue that must be saved
    public static LabQueue NewQueue()
    {
        return new LabQueue { Id = ObjectId.GenerateNewId().ToString() };
    }

    public async static Task<OperationResult> SaveNewQueue(LabQueue queue)
    {
        if (await Collection.CountDocumentsAsync(x => x.Name == queue.Name) != 0)
            return new OperationResult { Status = OperationStatus.AlreadyTaken };
        await Collection.InsertOneAsync(queue);

        return OperationResult.Ok;
    }

    public async static Task<LabQueueView?> GetQueueByName(string name)
    {
        var id = await Collection.Find(Builders<LabQueue>.Filter.Eq(x => x.Name, name)).Project(x => x.Id).SingleOrDefaultAsync();
        if (id is null)
            return null;
        return new LabQueueView(id);
    }

    public async Task AddUserToEnd(UserData user)
    {
        await RemoveUser(user);
        var member = new QueueMember { UserId = user.Id.ToString() };
        await Collection.UpdateOneAsync(Builders<LabQueue>.Filter.Eq("_id", Id),
            Builders<LabQueue>.Update.Push(x => x.Members, member));
        
    }

    public async Task RemoveUser(UserData user)
    {
        var result = await Collection.UpdateOneAsync(Builders<LabQueue>.Filter.Eq("_id", Id),
            Builders<LabQueue>.Update.PullFilter(x => x.Members, member => member.UserId == user.Id.ToString()));
    }

    public Task SetReady(UserData user) => SetReadyFlag(user, true);
    public Task SetUnready(UserData user) => SetReadyFlag(user, false);

    public async Task SetReadyFlag(UserData user, bool isReady)
    {
        await Collection.FindOneAndUpdateAsync(Builders<LabQueue>.Filter.Eq("_id", Id)
            & Builders<LabQueue>.Filter.ElemMatch(x => x.Members, m => m.UserId == user.Id.ToString()),
            Builders<LabQueue>.Update.Set(x => x.Members[-1].IsReady, isReady));
    }

    public async Task<LabQueue> GetRawQueue()
    {
        return await Collection.Find(Builders<LabQueue>.Filter.Eq("_id", Id)).FirstOrDefaultAsync();
    }

    public async Task<string?> GetPassword()
    {
        return await Collection.Find(Builders<LabQueue>.Filter.Eq("_id", Id)).Project(x => x.Password).FirstOrDefaultAsync();
    }

    public async Task<string?> GetName()
    {
        return await Collection.Find(Builders<LabQueue>.Filter.Eq("_id", Id)).Project(x => x.Name).FirstOrDefaultAsync();
    }

    public async Task<List<ObjectId>?> GetMainteiners()
    {
        return await Collection.Find(Builders<LabQueue>.Filter.Eq("_id", Id)).Project(x => x.Maintainers).FirstOrDefaultAsync();
    }
}
