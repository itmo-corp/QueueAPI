using MongoDB.Driver;
using QueueAPI.Models.DB.Auth;

namespace QueueAPI.Logic.Auth;

public static class APITokenManager
{
    private static IMongoCollection<APIToken> _collection = null!;
    public static IMongoCollection<APIToken> Collection => _collection ?? (_collection = Static.DB.GetCollection<APIToken>("APITokens"));

    public async static Task<APIToken> CreateToken(UserData userData)
    {
        var token = new APIToken { Id = Crypto.GetToken(Configs.APITokenSize), UserId = userData.Id.ToString() };
        await Collection.InsertOneAsync(token);
        return token;
    }

    public async static Task<APIToken?> GetToken(string tokenId)
    {
        return await Collection.Find(Builders<APIToken>.Filter.Eq("_id", tokenId))
            .SingleOrDefaultAsync();
    }

    public static async Task DeleteToken(APIToken token)
    {
        await Collection.FindOneAndDeleteAsync(Builders<APIToken>.Filter.Eq("_id", token.Id));
    }

    public static async Task<List<APIToken>> GetAllTokensOfUser(UserData user)
    {
        return await Collection.Find(Builders<APIToken>.Filter.Eq(x => x.UserId, user.Id.ToString())).ToListAsync();
    }
}
