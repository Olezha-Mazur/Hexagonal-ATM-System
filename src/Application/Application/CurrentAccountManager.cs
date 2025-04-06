using Contracts;
using Models;

namespace Application;

public class CurrentAccountManager : ICurrentAccountService
{
    public Account? Account { get; set; }
}