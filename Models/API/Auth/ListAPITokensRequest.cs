using System.ComponentModel.DataAnnotations;

namespace QueueAPI.Models.API.Auth;

public class ListAPITokensRequest
{
    [Required]
    public string UserTelegramId { get; set; } = null!;
    [Required]
    public string AuthorityToken { get; set; } = null!;
}
