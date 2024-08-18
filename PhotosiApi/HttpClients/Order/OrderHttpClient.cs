namespace PhotosiApi.HttpClients.Order;

public class OrderHttpClient : BaseHttpClient, IOrderHttpClient
{
    public OrderHttpClient(HttpClient httpClient) : base(httpClient)
    {
    }
}