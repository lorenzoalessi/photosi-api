namespace PhotosiApi.Exceptions;

public class BaseHttpClientException : Exception
{
    public BaseHttpClientException()
    {
    }

    public BaseHttpClientException(string message) : base(message)
    {
    }
}