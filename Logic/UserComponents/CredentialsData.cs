using QueueAPI.Models.DB.Auth;
using MongoDB.Bson;
using MongoDB.Driver;

namespace QueueAPI.Logic.PlayerComponents;

public class CredentialsData
{
    public ObjectId Id { get; init; }
    public UserCredentials? Data { get; set; }

    public CredentialsData(ObjectId id)
    {
        Id = id;
    }


    private static IMongoCollection<UserCredentials> _collection = null!;
    public static IMongoCollection<UserCredentials> Collection => _collection ?? (_collection = Static.DB.GetCollection<UserCredentials>("UserCredentials"));

    public async Task<CredentialsData> Load()
    {
        Data = await Collection.Find(Builders<UserCredentials>.Filter.Eq("_id", Id))
                    .SingleOrDefaultAsync();
        return this;
    }

    public async Task Save()
    {
        if (Data == null)
            throw new NullReferenceException("credentials cannot be null");
        if (Data.Id != Id.ToString())
            throw new InvalidOperationException("credentials object has wrong id");
        await Collection.ReplaceOneAsync(Builders<UserCredentials>.Filter.Eq("_id", Id), Data, new ReplaceOptions { IsUpsert = true });
    }
}
