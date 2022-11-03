using MongoDB.Driver;
namespace QueueAPI;

public static class Static
{
    static public MongoClient MongoClient { get; set; } = null!;
    private static IMongoDatabase _db = null!;
    static public IMongoDatabase DB => _db ?? (_db = MongoClient.GetDatabase(Configs.MongoDBName));
}
