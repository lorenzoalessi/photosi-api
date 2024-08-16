namespace PhotosiApi.HttpClients.User;

public class UserHttpClient : BaseHttpClient, IUserHttpClient
{
    public UserHttpClient(HttpClient httpClient) : base(httpClient)
    {
    }
}