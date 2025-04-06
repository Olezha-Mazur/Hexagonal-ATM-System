namespace Console.Exceptions;

public class NullCurrentAccountException : Exception
{
    public NullCurrentAccountException(string message)
        : base(message)
    {
    }

    public NullCurrentAccountException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public NullCurrentAccountException()
    {
    }
}