using MongoDB.Bson;
using QueueAPI.Logic.PlayerComponents;
using MongoDB.Driver;
using QueueAPI.Models.DB.Auth;
using QueueAPI.Logic.Auth;

namespace QueueAPI.Logic;

public class UserData
{
    public UserData(string id) : this(ObjectId.Parse(id))
    {
    }

    public UserData(ObjectId id)
    {
        Id = id;
    }

    public async static Task<UserData?> ByTelegramId(string telegramId)
    {
        var credentials = await CredentialsData.Collection.Find(Builders<UserCredentials>.Filter.Eq("TelegramId", telegramId))
                    .SingleOrDefaultAsync();
        if (credentials == null)
            return null;
        
        var userData = new UserData(credentials.Id);
        userData._credentialsData = new CredentialsData(userData.Id);
        userData._credentialsData.Data = credentials;
        return userData;
    }
    
    public ObjectId Id { get; init; }

    private CredentialsData _credentialsData = null!;
    public async Task<CredentialsData> GetCredentials() => _credentialsData ?? (_credentialsData = await new CredentialsData(Id).Load());

    public KnownQueuesView KnownQueues => new KnownQueuesView(Id);

    public async static Task<OperationResult<UserData>> CreateNewUser(string login)
    {
        if (await ByTelegramId(login) != null)
            return new OperationResult<UserData> { Status = OperationStatus.AlreadyTaken };
        var userData = new UserData(ObjectId.GenerateNewId());
        var credentials = await userData.GetCredentials();
        credentials.Data = new UserCredentials();
        credentials.Data.Id = userData.Id.ToString();
        credentials.Data.TelegramId = login;
        await credentials.Save();
        return new OperationResult<UserData> { Status = OperationStatus.Ok, Result = userData };
    }

}