using System.Text;
using Microsoft.Extensions.Primitives;

namespace QueueAPI;

public static class Configs
{
    public static string JwtKey => Environment.GetEnvironmentVariable("JWT_KEY") ?? string.Empty;
    public static string? JwtIssuer => Environment.GetEnvironmentVariable("JWT_ISSUER");
    public static string? JwtAudience => Environment.GetEnvironmentVariable("JWT_AUDIENCE");
    public static int JwtTokenDuration => int.Parse(Environment.GetEnvironmentVariable("JWT_TOKEN_DURATION") ?? "900"); // default is 15 minutes

    public static string MongoDBURL => Environment.GetEnvironmentVariable("MONGODB_URL") ?? "http://localhost:27017";
    public static string MongoDBName => Environment.GetEnvironmentVariable("MONGODB_DB_NAME") ?? "default";

    public static string[] AthorityTokens => (Environment.GetEnvironmentVariable("AUTHORITY_TOKENS") ?? "").Split(';', StringSplitOptions.RemoveEmptyEntries);
    public static int APITokenSize => int.Parse(Environment.GetEnvironmentVariable("API_TOKEN_SIZE") ?? "24"); // default is 15 minutes
    
}
