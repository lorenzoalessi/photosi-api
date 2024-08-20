using System.Net;
using PhotosiApi.Exceptions;

namespace PhotosiApi.HttpClients;

public class BaseHttpClient : IBaseHttpClient
{
    private readonly HttpClient _httpClient;

    public BaseHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> Get<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            throw new BaseHttpClientException(await response.Content.ReadAsStringAsync());

        if (response.StatusCode != HttpStatusCode.OK)
            return default;

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<bool> Put(string url, object body)
    {
        var response = await _httpClient.PutAsJsonAsync(url, body);
        return response.IsSuccessStatusCode;
    }

    public async Task<T?> Post<T>(string url, object body)
    {
        var response = await _httpClient.PostAsJsonAsync(url, body);

        if (!response.IsSuccessStatusCode)
            throw new BaseHttpClientException(await response.Content.ReadAsStringAsync());

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<bool> Delete(string url)
    {
        var response = await _httpClient.DeleteAsync(url);
        return response.IsSuccessStatusCode;
    }
}