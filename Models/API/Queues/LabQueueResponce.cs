using System.ComponentModel.DataAnnotations;

namespace QueueAPI.Models.API.Queues;

public class LabQueueResponce
{
    public string Name { get; set; } = null!;
    public List<QueueMemberResponce> Members { get; set; } = new List<QueueMemberResponce>();
    public List<string> Maintainers { get; set; } = new List<string>();
}
