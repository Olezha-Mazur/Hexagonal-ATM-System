using Contracts;
using Models;

namespace Abstractions.Repositories;

public interface IAccountRepository
{
    public Task<long> AddAccount(string password);

    public Task ChangeTotalMoneyAccount(long accountId, decimal totalMoney);

    public Task<FindAccountResult> TryFindAccount(long accountId, string password);

    public Task<Account?> TryGetAccount(long accountId);
}