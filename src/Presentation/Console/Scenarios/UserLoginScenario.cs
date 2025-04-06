﻿using Console.Scenarios.UserScenarios;
using Contracts;
using Spectre.Console;

namespace Console.Scenarios;

public class UserLoginScenario : IScenario
{
    private readonly IAccountService _accountService;
    private readonly IOperationHistoryService _operationHistoryService;
    private readonly ICurrentAccountService _currentAccountService;

    public string Name => "User";

    public UserLoginScenario(
        IAccountService accountService,
        IOperationHistoryService operationHistoryService,
        ICurrentAccountService currentAccountService)
    {
        _accountService = accountService;
        _operationHistoryService = operationHistoryService;
        _currentAccountService = currentAccountService;
    }

    public void Run()
    {
        long login = AnsiConsole.Ask<long>("Enter your account id");
        string password = AnsiConsole.Ask<string>("Enter your account password");

        FindAccountResult result = _accountService.TryFindAccount(login, password);

        switch (result)
        {
            case FindAccountResult.Success success:
                _currentAccountService.Account = success.Account;
                IEnumerable<IScenario> scenarios = GetPromt(_currentAccountService);
                SelectionPrompt<IScenario> selector = new SelectionPrompt<IScenario>()
                    .Title("Select action")
                    .AddChoices(scenarios)
                    .UseConverter(x => x.Name);

                IScenario scenario = AnsiConsole.Prompt(selector);
                scenario.Run();
                break;
            case FindAccountResult.NotFoundAccount:
                AnsiConsole.WriteLine("Account with this id don't exist.");
                bool confirmation = AnsiConsole.Prompt(
                    new TextPrompt<bool>("Would you like to create account?")
                        .AddChoice(true)
                        .AddChoice(false)
                        .DefaultValue(true)
                        .WithConverter(choice => choice ? "yes" : "no"));
                if (confirmation)
                {
                    var creation = new UserCreationAccountScenario(_accountService);
                    creation.Run();
                }

                break;
            case FindAccountResult.WrongPassword:
                AnsiConsole.WriteLine("Incorrect password entered.");
                break;
        }
    }

    private IEnumerable<IScenario> GetPromt(ICurrentAccountService current)
    {
        return
        [
            new UserAddMoneyScenario(_accountService, current),
            new UserCheckBalanceScenario(current),
            new UserGetOperationsScenario(_operationHistoryService, current),
            new UserWithdrawalScenario(_accountService, current)
        ];
    }
}