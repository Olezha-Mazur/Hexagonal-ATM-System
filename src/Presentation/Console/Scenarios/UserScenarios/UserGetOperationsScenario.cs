using Console.Exceptions;
using Contracts;
using Models.Operations;
using Spectre.Console;
using System.Globalization;

namespace Console.Scenarios.UserScenarios;

public class UserGetOperationsScenario : IScenario
{
    private readonly ICurrentAccountService _currentAccountService;
    private readonly IOperationHistoryService _operationHistoryService;

    public UserGetOperationsScenario(
        IOperationHistoryService operationHistoryService,
        ICurrentAccountService currentAccountService)
    {
        _operationHistoryService = operationHistoryService;
        _currentAccountService = currentAccountService;
    }

    public string Name => "Get history";

    public void Run()
    {
        if (_currentAccountService.Account is null)
        {
            throw new NullCurrentAccountException();
        }

        IEnumerable<Models.Operations.Operation> operations = _operationHistoryService
            .GetOperationsByAccountId(_currentAccountService.Account.Id);
        var table = new Table();
        table.AddColumns("Date", "Id", "Amount before", "Amount after", "Type");
        foreach (Operation operation in operations)
        {
            table.AddRow(
                operation.Date.ToString(CultureInfo.InvariantCulture),
                operation.AccountId.ToString(),
                operation.AmountBefore.ToString() ?? string.Empty,
                operation.AmountAfter.ToString() ?? string.Empty,
                operation.Type.ToString());
        }

        AnsiConsole.Write(table);
    }
}