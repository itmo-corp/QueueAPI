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
        var credentials = await CredentialsView.Collection.Find(Builders<UserCredentials>.Filter.Eq("TelegramId", telegramId))
                    .SingleOrDefaultAsync();
        if (credentials == null)
            return null;

        return new UserData(credentials.Id);
    }

    public ObjectId Id { get; init; }

    public KnownQueuesView KnownQueues => new KnownQueuesView(Id);
    public CredentialsView Credentials => new CredentialsView(Id);

    public async static Task<OperationResult<UserData>> CreateNewUser(Models.API.Auth.UserRegisterRequest request)
    {
        if (await ByTelegramId(request.TelegramId) != null)
            return new OperationResult<UserData> { Status = OperationStatus.AlreadyTaken };

        var userData = new UserData(ObjectId.GenerateNewId());
        var credentials = userData.Credentials.GetNewCredentials();
        credentials.Id = userData.Id.ToString();
        credentials.TelegramId = request.TelegramId;
        credentials.DisplayName = request.Name;
        await userData.Credentials.SaveNewCredentials(credentials);
        return OperationResult<UserData>.Ok(userData);
    }

}