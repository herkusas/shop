namespace Shop.Exceptions;

public class NoSuchOrderException : Exception
{
    public NoSuchOrderException()
    {
    }

    public NoSuchOrderException(string message)
        : base(message)
    {
    }

    public NoSuchOrderException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
