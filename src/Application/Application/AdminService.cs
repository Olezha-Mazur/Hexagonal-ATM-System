using Contracts;
using Models;

namespace Application;

public class AdminService : IAdminService
{
    private readonly SystemPassword _password;

    public AdminService(SystemPassword password)
    {
        _password = password;
    }

    public bool IsCorrectPassword(string password)
    {
        return password == _password.Password;
    }
}