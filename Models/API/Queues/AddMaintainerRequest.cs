
using System.ComponentModel.DataAnnotations;

namespace QueueAPI.Models.API.Queues;

public class AddMaintainerRequest
{
    [Required]
    public string QueueId { get; set; } = null!;
    [Required]
    public string MaintainerTelegramId { get; set; } = null!;
}
