using Abstractions.Repositories;
using Contracts;
using Models.Operations;

namespace Application;

public class OperationHistoryService : IOperationHistoryService
{
    private readonly IOperationRepository _operationRepository;

    public OperationHistoryService(IOperationRepository operationRepository)
    {
        _operationRepository = operationRepository;
    }

    public IEnumerable<Operation> GetOperationsByAccountId(long accountId)
    {
        return _operationRepository.GetOperationsByAccountId(accountId).Result;
    }
}