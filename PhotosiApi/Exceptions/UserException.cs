using System.Diagnostics.CodeAnalysis;

namespace PhotosiApi.Exceptions;

[ExcludeFromCodeCoverage]
public class UserException : Exception
{
    public UserException()
    {
    }

    public UserException(string message) : base(message)
    {
    }
}