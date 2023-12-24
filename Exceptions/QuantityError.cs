namespace shoppingcart.Exceptions;

public class QuantityError : Exception
{
    public QuantityError(string? message) : base(message)
    {
    }
}