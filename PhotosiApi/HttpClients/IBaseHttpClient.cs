namespace PhotosiApi.HttpClients;

public interface IBaseHttpClient
{
    Task<T?> Get<T>(string url);
    
    Task<bool> Put(string url, object body);

    Task<T?> Post<T>(string url, object body);
    
    Task<bool> Delete(string url);
}