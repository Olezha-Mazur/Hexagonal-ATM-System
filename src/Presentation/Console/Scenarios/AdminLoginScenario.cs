using Contracts;
using Spectre.Console;

namespace Console.Scenarios;

public class AdminLoginScenario : IScenario
{
    private readonly IAdminService _adminService;

    public AdminLoginScenario(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public string Name => "Login as admin";

    public void Run()
    {
        string systemPassword = AnsiConsole.Ask<string>("Enter system password");
        if (_adminService.IsCorrectPassword(systemPassword))
        {
            AnsiConsole.WriteLine($"The system password is correct");
        }
        else
        {
            Environment.Exit(1);
        }
    }
}