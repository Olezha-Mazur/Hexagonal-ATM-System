using Console.Exceptions;
using Contracts;
using Spectre.Console;

namespace Console.Scenarios.UserScenarios;

public class UserCheckBalanceScenario : IScenario
{
    private readonly ICurrentAccountService _currentAccountService;

    public UserCheckBalanceScenario(ICurrentAccountService currentAccountService)
    {
        _currentAccountService = currentAccountService;
    }

    public string Name => "Check balance";

    public void Run()
    {
        if (_currentAccountService.Account is null)
        {
            throw new NullCurrentAccountException();
        }

        AnsiConsole.WriteLine($"Current balance: {_currentAccountService.Account.TotalMoney}");
    }
}