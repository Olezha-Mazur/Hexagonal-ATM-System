using Models.Operations;

namespace Contracts;

public interface IOperationHistoryService
{
    public IEnumerable<Operation> GetOperationsByAccountId(long accountId);
}