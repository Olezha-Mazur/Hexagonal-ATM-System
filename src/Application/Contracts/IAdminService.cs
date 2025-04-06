namespace Contracts;

public interface IAdminService
{
    bool IsCorrectPassword(string password);
}