namespace Application;

public record ChangeBalanceResult
{
    private ChangeBalanceResult() { }

    public sealed record Success() : ChangeBalanceResult;

    public sealed record NotEnoughMoney : ChangeBalanceResult;

    public sealed record NullAccountResult : ChangeBalanceResult;
}