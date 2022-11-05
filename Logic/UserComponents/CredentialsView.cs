using QueueAPI.Models.DB.Auth;
using MongoDB.Bson;
using MongoDB.Driver;

namespace QueueAPI.Logic.PlayerComponents;

public class CredentialsView
{
    public ObjectId Id { get; init; }

    public CredentialsView(string id) : this(ObjectId.Parse(id)) { }
    public CredentialsView(ObjectId id)
    {
        Id = id;
    }


    private static IMongoCollection<UserCredentials> _collection = null!;
    public static IMongoCollection<UserCredentials> Collection => _collection ?? (_collection = Static.DB.GetCollection<UserCredentials>("UserCredentials"));

    public UserCredentials GetNewCredentials()
    {
        return new UserCredentials { Id = Id.ToString() };
    }

    public async Task SaveNewCredentials(UserCredentials newCredentials)
    {
        if (newCredentials.Id != Id.ToString())
            throw new InvalidOperationException("Changed id of new credentials");
        await Collection.InsertOneAsync(newCredentials);
    }

    public async Task<string> GetTelegramId()
    {
        return await FindThis().Project(x => x.TelegramId).FirstAsync();
    }

    public async Task<string> GetDisplayName()
    {
        return await FindThis().Project(x => x.DisplayName).FirstAsync();
    }

    public async Task SetTelegramId(string telegramId)
    {
        await UpdateThisSetAsync(x => x.TelegramId, telegramId);
    }

    public async Task SetDisplayName(string displayName)
    {
        await UpdateThisSetAsync(x => x.DisplayName, displayName);
    }

    public async Task<UserCredentials?> GetRawCredentials()
    {
        return await FindThis().FirstOrDefaultAsync();
    }

    private IFindFluent<UserCredentials, UserCredentials> FindThis() => Collection.Find(EqId());
    private Task<UpdateResult> UpdateThisSetAsync<TField>(System.Linq.Expressions.Expression<Func<UserCredentials, TField>> field, TField value)
    {
        return Collection.UpdateOneAsync(EqId(), Builders<UserCredentials>.Update.Set(field, value));
    }
    private FilterDefinition<UserCredentials> EqId() => Builders<UserCredentials>.Filter.Eq("_id", Id);
}
