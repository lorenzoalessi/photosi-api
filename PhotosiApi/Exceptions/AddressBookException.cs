namespace PhotosiApi.Exceptions;

public class AddressBookException : Exception
{
    public AddressBookException()
    {
    }

    public AddressBookException(string message) : base(message)
    {
    }
}