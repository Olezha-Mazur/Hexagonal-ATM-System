using Console.Exceptions;
using Contracts;
using Spectre.Console;

namespace Console.Scenarios.UserScenarios;

public class UserAddMoneyScenario : IScenario
{
    private readonly IAccountService _accountService;
    private readonly ICurrentAccountService _currentAccountService;

    public UserAddMoneyScenario(
        IAccountService accountService,
        ICurrentAccountService currentAccountService)
    {
        _accountService = accountService;
        _currentAccountService = currentAccountService;
    }

    public string Name => "Add money";

    public void Run()
    {
        decimal amount = AnsiConsole.Ask<decimal>("Enter the amount");
        if (_currentAccountService.Account is null)
        {
            throw new NullCurrentAccountException();
        }

        _accountService.ReplenishmentMoneyAccount(_currentAccountService.Account.Id, amount);
        AnsiConsole.WriteLine($"Money successfully added to the account {_currentAccountService.Account.Id}");
    }
}