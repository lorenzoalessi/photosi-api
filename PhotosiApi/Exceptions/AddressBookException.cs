using System.Diagnostics.CodeAnalysis;

namespace PhotosiApi.Exceptions;

[ExcludeFromCodeCoverage]
public class AddressBookException : Exception
{
    public AddressBookException()
    {
    }

    public AddressBookException(string message) : base(message)
    {
    }
}