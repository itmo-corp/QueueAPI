using System.ComponentModel.DataAnnotations;

namespace QueueAPI.Models.API.Auth;

public class DeleteApiTokenRequest
{
    [Required]
    public string Token { get; set; } = null!;
    [Required]
    public string AuthorityToken { get; set; } = null!;
}
