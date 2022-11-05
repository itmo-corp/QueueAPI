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
    public Task<string> GetDisplayName()
    {
        var user = GetUser();
        return user.Credentials.GetDisplayName();
    }

    [HttpPost("setDisplayName")]
    public Task SetDisplayName([FromBody] string name)
    {
        var user = GetUser();
        return user.Credentials.SetDisplayName(name);
    }

    private UserData GetUser() => new UserData(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
}