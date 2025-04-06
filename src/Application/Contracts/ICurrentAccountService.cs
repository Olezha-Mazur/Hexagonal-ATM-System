using Models;

namespace Contracts;

public interface ICurrentAccountService
{
    Account? Account { get; set; }
}