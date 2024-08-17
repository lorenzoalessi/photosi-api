namespace PhotosiApi.HttpClients.AddressBook;

public class AddressBookHttpClient : BaseHttpClient, IAddressBookHttpClient
{
    public AddressBookHttpClient(HttpClient httpClient) : base(httpClient)
    {
    }
}