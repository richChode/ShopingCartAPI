namespace shoppingcart.Exceptions;

public class AlreadyExistError : Exception
{
    public AlreadyExistError(string? message) : base(message)
    {
    }
}