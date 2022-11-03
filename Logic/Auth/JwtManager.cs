using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using QueueAPI.Models.API.Auth;
using Microsoft.IdentityModel.Tokens;

namespace QueueAPI.Logic.Auth;

public static class JwtManager
{
    public static JwtToken GenerateToken(UserData playerData)
    {   
        return new JwtToken
        {
            Token = GetNewToken(playerData),
            Expires = DateTime.Now.AddSeconds(Configs.JwtTokenDuration)
        };
    }

    private static string GetNewToken(UserData playerData)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configs.JwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, playerData.Id.ToString()),
        };

        var token = new JwtSecurityToken(Configs.JwtIssuer,
            Configs.JwtAudience,
            claims,
            expires: DateTime.Now.AddSeconds(Configs.JwtTokenDuration),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
