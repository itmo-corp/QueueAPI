using System.ComponentModel.DataAnnotations;

namespace QueueAPI.Models.API.Auth;

public class JwtToken
{
    [Required]
    public string Token { get; set; } = null!;
    public DateTime? Expires { get; set; }
}