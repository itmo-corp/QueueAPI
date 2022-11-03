using System.ComponentModel.DataAnnotations;

namespace QueueAPI.Models.API.Auth;

public class GenerateNewAPITokenRequest
{
    [Required]
    public string UserTelegramId { get; set; } = null!;
    [Required]
    public string AuthorityToken { get; set; } = null!;
}
