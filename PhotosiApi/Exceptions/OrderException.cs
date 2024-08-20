using System.Diagnostics.CodeAnalysis;

namespace PhotosiApi.Exceptions;

[ExcludeFromCodeCoverage]
public class OrderException : Exception
{
    public OrderException()
    {
    }

    public OrderException(string message) : base(message)
    {
    }
}