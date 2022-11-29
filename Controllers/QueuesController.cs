using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueAPI.Logic;
using QueueAPI.Models.API.Queues;
using QueueAPI.Logic.Queues;
using System.Security.Claims;

namespace QueueAPI.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class QueuesController : ControllerBase
{
    [HttpPost("create")]
    public async Task<OperationResult<string>> CreateQueue([FromBody] CreateQueueRequest request)
    {
        var user = GetUser();

        var queue = LabQueueView.NewQueue();
        queue.Name = request.Name;
        queue.Password = request.Password;
        queue.Maintainers.Add(user.Id);
        var result = await LabQueueView.SaveNewQueue(queue);

        if (result.Status != OperationStatus.Ok)
            return new OperationResult<string> { Status = result.Status };

        await user.KnownQueues.Add(queue.Id);

        return OperationResult<string>.Ok(queue.Id);
    }

    [HttpPost("add")]
    public async Task<OperationResult<string>> AddQueue([FromBody] AddQueueRequest request)
    {
        var user = GetUser();

        var queue = await LabQueueView.GetQueueByName(request.Name);

        if (queue is null)
            return new OperationResult<string> { Status = OperationStatus.NotFound };

        var passwordResult = await queue.GetPassword();

        if (passwordResult.Data != request.Password)
            return new OperationResult<string> { Status = OperationStatus.Wrong };

        await user.KnownQueues.Add(queue.Id.ToString());

        return OperationResult<string>.Ok(queue.Id.ToString());
    }

    [HttpPost("forget")]
    public async Task<OperationResult> ForgetQueue([FromBody] string queueId)
    {
        var user = GetUser();

        if (!await user.KnownQueues.Contains(queueId))
            return new OperationResult { Status = OperationStatus.Forbid };

        var queue = new LabQueueView(queueId);

        await user.KnownQueues.Remove(queueId);

        await queue.RemoveUser(user);

        return OperationResult.Ok;
    }

    [HttpPost("join")]
    public async Task<OperationResult> JoinQueue([FromBody] string queueId)
    {
        var user = GetUser();

        if (!await user.KnownQueues.Contains(queueId))
            return new OperationResult { Status = OperationStatus.Forbid };

        var queue = new LabQueueView(queueId); // TODO add check on entry in DB

        await queue.AddUserToEnd(user);

        return OperationResult.Ok;
    }

    [HttpPost("leave")]
    public async Task<OperationResult> LeaveQueue([FromBody] string queueId)
    {
        var user = GetUser();

        if (!await user.KnownQueues.Contains(queueId))
            return new OperationResult { Status = OperationStatus.Forbid };

        var queue = new LabQueueView(queueId); // TODO add check on entry in DB

        await queue.RemoveUser(user);

        return OperationResult.Ok;
    }

    [HttpPost("ready")]
    public async Task<OperationResult> SetReady([FromBody] string queueId)
    {
        var user = GetUser();

        if (!await user.KnownQueues.Contains(queueId))
            return new OperationResult { Status = OperationStatus.Forbid };

        var queue = new LabQueueView(queueId); // TODO add check on entry in DB

        await queue.SetReady(user);

        return OperationResult.Ok;
    }

    [HttpPost("unready")]
    public async Task<OperationResult> SetUnready([FromBody] string queueId)
    {
        var user = GetUser();

        if (!await user.KnownQueues.Contains(queueId))
            return new OperationResult { Status = OperationStatus.Forbid };

        var queue = new LabQueueView(queueId); // TODO add check on entry in DB

        await queue.SetUnready(user);

        return OperationResult.Ok;
    }

    [HttpGet("getKnownQueues")]
    public async Task<OperationResult<string[]>> GetMyQueues()
    {
        var user = GetUser();

        var knownQueues = await user.KnownQueues.Get();

        return OperationResult<string[]>.Ok(knownQueues.Items.Select(x => x.ToString()).ToArray());
    }

    [HttpGet("getKnownQueuesNames")]
    public async Task<OperationResult<string[]>> GetMyQueuesNames()
    {
        var user = GetUser();

        var knownQueuesIds = await user.KnownQueues.Get();

        var queues = knownQueuesIds.Items.Select(x => new LabQueueView(x));

        var names = OperationResultUtils.GetOkData(await Task.WhenAll(queues.Select(x => x.GetName())));

        return OperationResult<string[]>.Ok(names.ToArray());
    }

    [HttpPost("getQueueName")]
    public async Task<OperationResult<string>> GetQueueName([FromBody] string queueId)
    {
        var user = GetUser();

        if (!await user.KnownQueues.Contains(queueId))
            return new OperationResult<string> { Status = OperationStatus.Forbid };

        var queue = new LabQueueView(queueId);

        var nameResult = await queue.GetName();

        if (nameResult.Status != OperationStatus.Ok)
            return new OperationResult<string> { Status = nameResult.Status };

        return OperationResult<string>.Ok(nameResult.Data!);
    }

    [HttpPost("getQueueInfo")]
    public async Task<OperationResult<LabQueueResponce>> GetQueueInfo([FromBody] string queueId)
    {
        var user = GetUser();

        if (!await user.KnownQueues.Contains(queueId))
            return new OperationResult<LabQueueResponce> { Status = OperationStatus.Forbid };

        var queue = new LabQueueView(queueId);

        var rawQueue = await queue.GetRawQueue();

        if (rawQueue is null)
            return new OperationResult<LabQueueResponce> { Status = OperationStatus.NotFound };

        return OperationResult<LabQueueResponce>.Ok(await QueueForResponceConverter.Convert(rawQueue));
    }

    [HttpPost("getQueueIdByName")]
    public async Task<OperationResult<string>> GetQueueIdByName([FromBody] string queueName)
    {
        var user = GetUser();

        var queue = await LabQueueView.GetQueueByName(queueName);

        if (queue is null || !await user.KnownQueues.Contains(queue.Id.ToString()))
            return new OperationResult<string> { Status = OperationStatus.Forbid };

        return OperationResult<string>.Ok(queue.Id.ToString());
    }

    [HttpPost("getImInQueue")]
    public async Task<OperationResult<bool>> GetImInQueue([FromBody] string queueId)
    {
        var user = GetUser();

        if (!await user.KnownQueues.Contains(queueId))
            return new OperationResult<bool> { Status = OperationStatus.Forbid };

        var queue = new LabQueueView(queueId);

        var queueMembersResult = await queue.GetMembers();

        if (queueMembersResult.Status != OperationStatus.Ok)
            return new OperationResult<bool> { Status = queueMembersResult.Status };

        return OperationResult<bool>.Ok(queueMembersResult.Data!.Select(x => x.UserId).Contains(user.Id.ToString()));
    }


    private UserData GetUser() => new UserData(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
}