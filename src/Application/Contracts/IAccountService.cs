using Application;

namespace Contracts;

public interface IAccountService
{
    public long AddAccount(string password);

    public ChangeBalanceResult ReplenishmentMoneyAccount(long accountId, decimal totalMoney);

    public ChangeBalanceResult WithDrawalMoneyAccount(long accountId, decimal totalMoney);

    public FindAccountResult TryFindAccount(long accountId, string password);
}