using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueAPI.Logic;
using QueueAPI.Models.API.Queues;
using QueueAPI.Logic.Queues;
using System.Security.Claims;

namespace QueueAPI.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet("getDisplayName")]
    public async Task<OperationResult<string>> GetDisplayName()
    {
        var user = GetUser();
        return OperationResult<string>.Ok(await user.Credentials.GetDisplayName());
    }

    [HttpPost("setDisplayName")]
    public async Task<OperationResult> SetDisplayName([FromBody] string name)
    {
        var user = GetUser();
        await user.Credentials.SetDisplayName(name);
        return OperationResult.Ok;
    }

    private UserData GetUser() => new UserData(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
}