namespace Identity.Domain.Exceptions;

public class ApplicationException : Exception
{
    public ApplicationException() : base()
    {
    }

    public ApplicationException(string message) : base(message)
    {
    }

    public ApplicationException(string message, Exception inner) : base(message, inner)
    {
    }
}
