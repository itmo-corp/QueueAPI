using System.ComponentModel.DataAnnotations;

namespace QueueAPI.Models.API.Queues;

public class CreateQueueRequest
{
    [Required]
    public string Name { get; set; } = null!;
    public string? Password { get; set; } = null;
}
