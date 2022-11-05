
using System.ComponentModel.DataAnnotations;

namespace QueueAPI.Models.API.Auth;

[System.Serializable]
public class UserRegisterRequest
{
    [Required(ErrorMessage = "Id required")]
    public string TelegramId { get; set; } = null!;
    [Required(ErrorMessage = "Name required")]
    public string Name { get; set; } = null!;
    public string AuthorityToken { get; set; } = null!;
}
