﻿using Abstractions.Repositories;
using Contracts;
using Models;
using Models.Operations;

namespace Application;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IOperationRepository _operationRepository;

    public AccountService(IAccountRepository accountRepository, IOperationRepository operationRepository)
    {
        _accountRepository = accountRepository;
        _operationRepository = operationRepository;
    }

    public long AddAccount(string password)
    {
        long id = _accountRepository.AddAccount(password).Result;
        _operationRepository.AddOperation(id, null, 0, OperationType.Creation);
        return id;
    }

    public ChangeBalanceResult ReplenishmentMoneyAccount(long accountId, decimal totalMoney)
    {
        Account? account = _accountRepository.TryGetAccount(accountId).Result;
        if (account is null)
        {
            return new ChangeBalanceResult.NullAccountResult();
        }

        _operationRepository.AddOperation(
            account.Id,
            account.TotalMoney,
            account.TotalMoney + totalMoney,
            OperationType.Replenishment);
        _accountRepository.ChangeTotalMoneyAccount(account.Id, account.TotalMoney + totalMoney);
        return new ChangeBalanceResult.Success();
    }

    public ChangeBalanceResult WithDrawalMoneyAccount(long accountId, decimal totalMoney)
    {
        Account? account = _accountRepository.TryGetAccount(accountId).Result;
        if (account is null)
        {
            return new ChangeBalanceResult.NullAccountResult();
        }

        if (account.TotalMoney - totalMoney < 0)
        {
            return new ChangeBalanceResult.NotEnoughMoney();
        }

        _operationRepository.AddOperation(
            account.Id,
            account.TotalMoney,
            account.TotalMoney - totalMoney,
            OperationType.Withdrawal);
        _accountRepository.ChangeTotalMoneyAccount(account.Id, account.TotalMoney - totalMoney);
        return new ChangeBalanceResult.Success();
    }

    public FindAccountResult TryFindAccount(long accountId, string password)
    {
        return _accountRepository.TryFindAccount(accountId, password).Result;
    }
}