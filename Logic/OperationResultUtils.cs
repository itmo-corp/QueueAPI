
namespace QueueAPI.Logic;

public class OperationResultUtils
{
    public static IEnumerable<T> GetOkData<T>(IEnumerable<OperationResult<T>> results)
    {
        return results.Where(r => r.Status == OperationStatus.Ok).Select(r => r.Data!);
    }
}