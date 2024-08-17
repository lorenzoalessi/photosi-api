namespace PhotosiApi.HttpClients.Product;

public class ProductHttpClient : BaseHttpClient, IProductHttpClient
{
    public ProductHttpClient(HttpClient httpClient) : base(httpClient)
    {
    }
}