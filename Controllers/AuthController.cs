using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueAPI.Logic;
using QueueAPI.Models.API.Auth;
using QueueAPI.Logic.Auth;
using QueueAPI.Models.DB.Auth;

namespace QueueAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<OperationResult> Register([FromBody] UserRegisterRequest request)
    {
        if (!IsAuthorityTokenValid(request.AuthorityToken))
            return OperationResult.Wrong;

        var result = await UserData.CreateNewUser(request);

        return new OperationResult { Status = result.Status };
    }

    [AllowAnonymous]
    [HttpPost("newApiToken")]
    public async Task<OperationResult<string>> NewAPIToken([FromBody] GenerateNewAPITokenRequest request)
    {
        if (!IsAuthorityTokenValid(request.AuthorityToken))
            return new OperationResult<string> { Status = OperationStatus.Wrong };

        var user = await UserData.ByTelegramId(request.UserTelegramId!);

        if (user is null)
            return new OperationResult<string> { Status = OperationStatus.NotExists };

        var token = await APITokenManager.CreateToken(user);

        return OperationResult<string>.Ok(token.Id);
    }

    [AllowAnonymous]
    [HttpPost("deleteApiToken")]
    public async Task<OperationResult> DeleteAPIToken([FromBody] DeleteApiTokenRequest request)
    {
        if (!IsAuthorityTokenValid(request.AuthorityToken))
            return OperationResult.Wrong;

        var token = await APITokenManager.GetToken(request.Token);

        if (token is null)
            return OperationResult.NotExists;
        
        await APITokenManager.DeleteToken(token);

        return OperationResult.Ok;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<OperationResult<JwtToken>> Login([FromBody] UserLoginRequest request)
    {
        APIToken? apiToken = await APITokenManager.GetToken(request.APIToken!);

        if (apiToken is null)
            return new OperationResult<JwtToken> { Status = OperationStatus.NotExists };

        var user = new UserData(apiToken.UserId);

        var token = JwtManager.GenerateToken(user);

        return OperationResult<JwtToken>.Ok(token);
    }

    [AllowAnonymous]
    [HttpPost("listApiTokens")]
    public async Task<OperationResult<string[]>> ListAPITokens([FromBody] ListAPITokensRequest request)
    {
        if (!IsAuthorityTokenValid(request.AuthorityToken))
            return new OperationResult<string[]> { Status = OperationStatus.Wrong };
        
        var user = await UserData.ByTelegramId(request.UserTelegramId);

        if (user is null)
            return new OperationResult<string[]> { Status = OperationStatus.NotExists };
        
        var tokens = await APITokenManager.GetAllTokensOfUser(user);

        return OperationResult<string[]>.Ok(tokens.Select(x => x.Id).ToArray());
    }

    private bool IsAuthorityTokenValid(string token) => Configs.AthorityTokens.Contains(token);
}