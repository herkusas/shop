namespace Shop.Exceptions;

public class NoSuchClientException : Exception
{
    public NoSuchClientException()
    {
    }

    public NoSuchClientException(string message)
        : base(message)
    {
    }

    public NoSuchClientException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
