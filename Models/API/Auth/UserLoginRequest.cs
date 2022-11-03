
using System.ComponentModel.DataAnnotations;

namespace QueueAPI.Models.API.Auth;

public class UserLoginRequest
{
    [Required]
    public string? APIToken { get; set; }
}
