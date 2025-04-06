using Application;
using Console.Exceptions;
using Contracts;
using Spectre.Console;

namespace Console.Scenarios.UserScenarios;

public class UserWithdrawalScenario : IScenario
{
    private readonly IAccountService _accountService;
    private readonly ICurrentAccountService _currentAccountService;

    public UserWithdrawalScenario(
        IAccountService accountService,
        ICurrentAccountService currentAccountService)
    {
        _accountService = accountService;
        _currentAccountService = currentAccountService;
    }

    public string Name => "Update amount";

    public void Run()
    {
        decimal amount = AnsiConsole.Ask<decimal>("Enter the amount to be debited");
        if (_currentAccountService.Account is null)
        {
            throw new NullCurrentAccountException();
        }

        ChangeBalanceResult result = _accountService
            .WithDrawalMoneyAccount(_currentAccountService.Account.Id, amount);
        switch (result)
        {
            case ChangeBalanceResult.Success:
                AnsiConsole.WriteLine($"Money successfully debited from the account {_currentAccountService.Account.Id}");
                break;
            case ChangeBalanceResult.NotEnoughMoney:
                AnsiConsole.WriteLine($"There are not enough funds in the account {_currentAccountService.Account.Id}");
                break;
            case ChangeBalanceResult.NullAccountResult:
                AnsiConsole.WriteLine($"Not fount account {_currentAccountService.Account.Id}");
                break;
        }
    }
}