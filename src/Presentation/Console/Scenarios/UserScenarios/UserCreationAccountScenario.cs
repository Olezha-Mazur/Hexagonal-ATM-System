using Contracts;
using Spectre.Console;

namespace Console.Scenarios.UserScenarios;

public class UserCreationAccountScenario : IScenario
{
    public string Name => "Create account";

    private readonly IAccountService _accountService;

    public UserCreationAccountScenario(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public void Run()
    {
        string pin = AnsiConsole.Ask<string>("Enter account password");
        long id = _accountService.AddAccount(pin);
        AnsiConsole.WriteLine($"Account assigned id {id}");
    }
}