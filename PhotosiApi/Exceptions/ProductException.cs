namespace PhotosiApi.Exceptions;

public class ProductException : Exception
{
    public ProductException()
    {
    }

    public ProductException(string message) : base(message)
    {
    }
}