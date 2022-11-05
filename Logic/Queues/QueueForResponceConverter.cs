using MongoDB.Bson;
using MongoDB.Driver;
using QueueAPI.Models.API.Queues;
using QueueAPI.Models.DB.Queues;

namespace QueueAPI.Logic.Queues;

public class QueueForResponceConverter
{
    public async static Task<LabQueueResponce> Convert(LabQueue queue)
    {
        var result = new LabQueueResponce();
        result.Name = queue.Name;
        result.Members = (await Task.WhenAll(queue.Members.Select(x => Convert(x)))).ToList();
        result.Maintainers = (await Task.WhenAll(queue.Maintainers.Select(x => GetUserDisplayName(x)))).ToList();
        return result;
    }

    public async static Task<QueueMemberResponce> Convert(QueueMember member)
    {
        var result = new QueueMemberResponce();
        result.DisplayName = await new UserData(member.UserId).Credentials.GetDisplayName();
        result.IsReady = member.IsReady;
        return result;
    }

    private async static Task<string> GetUserDisplayName(ObjectId userId)
    {
        var user = new UserData(userId);
        return await user.Credentials.GetDisplayName();
    }
}
