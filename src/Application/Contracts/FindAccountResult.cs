using Models;

namespace Contracts;

public abstract record FindAccountResult
{
    private FindAccountResult() { }

    public sealed record Success(Account Account) : FindAccountResult;

    public sealed record NotFoundAccount : FindAccountResult;

    public sealed record WrongPassword : FindAccountResult;
}