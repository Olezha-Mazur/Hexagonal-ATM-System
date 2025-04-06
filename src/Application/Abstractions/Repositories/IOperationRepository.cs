using Azure;
using Models.Operations;
using Operation = Models.Operations.Operation;

namespace Abstractions.Repositories;

public interface IOperationRepository
{
    public Task<IEnumerable<Operation>> GetOperationsByAccountId(long accountId);

    public Task AddOperation(long accountId, decimal? amountBefore, decimal? amountAfter, OperationType type);
}