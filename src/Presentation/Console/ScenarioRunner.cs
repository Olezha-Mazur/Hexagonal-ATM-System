using Console.Scenarios;
using Contracts;
using Spectre.Console;

namespace Console;

public class ScenarioRunner
{
    private readonly List<IScenario> _loginScenarios = [];

    private readonly IAccountService _accountService;
    private readonly IOperationHistoryService _operationHistoryService;
    private readonly ICurrentAccountService _currentAccountService;
    private readonly IAdminService _adminService;

    public ScenarioRunner(
        IAccountService accountService,
        IOperationHistoryService operationHistoryService,
        ICurrentAccountService currentAccountService,
        IAdminService adminService)
    {
        _accountService = accountService;
        _operationHistoryService = operationHistoryService;
        _currentAccountService = currentAccountService;
        _adminService = adminService;
        _loginScenarios.Add(new AdminLoginScenario(_adminService));
        _loginScenarios.Add(new UserLoginScenario(_accountService, _operationHistoryService, _currentAccountService));
    }

    public ScenarioRunner AddLoginSelector(IScenario newSelector)
    {
        _loginScenarios.Add(newSelector);
        return this;
    }

    public void Run()
    {
        SelectionPrompt<IScenario> selector = new SelectionPrompt<IScenario>()
            .Title("Select operating mode")
            .AddChoices(_loginScenarios)
            .UseConverter(x => x.Name);

        IScenario scenario = AnsiConsole.Prompt(selector);
        scenario.Run();
    }
}