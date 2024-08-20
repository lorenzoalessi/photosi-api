using System.Diagnostics.CodeAnalysis;

namespace PhotosiApi.Exceptions;

[ExcludeFromCodeCoverage]
public class ProductException : Exception
{
    public ProductException()
    {
    }

    public ProductException(string message) : base(message)
    {
    }
}