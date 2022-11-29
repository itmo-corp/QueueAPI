
namespace QueueAPI.Logic;

public class OperationResult<T>
{
    public OperationStatus Status { get; init; } = OperationStatus.None;
    public T? Result { get; init; }

    public static OperationResult<T> Ok(T result) => new OperationResult<T> { Status = OperationStatus.Ok, Result = result };
}

public class OperationResult
{
    public OperationStatus Status { get; init; } = OperationStatus.None;

    public static readonly OperationResult Ok = new OperationResult { Status = OperationStatus.Ok };
    public static readonly OperationResult AlreadyTaken = new OperationResult { Status = OperationStatus.AlreadyTaken };
    public static readonly OperationResult Wrong = new OperationResult { Status = OperationStatus.Wrong };
    public static readonly OperationResult NotFound = new OperationResult { Status = OperationStatus.NotFound };
}

public enum OperationStatus
{
    None = 0,
    Ok = 1,
    Error = 2,
    TooManyAttempets = 3,
    Wrong = 4,
    AlreadyTaken = 5,
    NotFound = 6,
    Forbid = 7,
}
