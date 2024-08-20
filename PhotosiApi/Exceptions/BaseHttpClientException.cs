using System.Diagnostics.CodeAnalysis;

namespace PhotosiApi.Exceptions;

[ExcludeFromCodeCoverage]
public class BaseHttpClientException : Exception
{
    public BaseHttpClientException()
    {
    }

    public BaseHttpClientException(string message) : base(message)
    {
    }
}