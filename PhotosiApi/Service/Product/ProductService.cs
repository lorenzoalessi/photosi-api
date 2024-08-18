using Microsoft.Extensions.Options;
using PhotosiApi.Dto.Product;
using PhotosiApi.Exceptions;
using PhotosiApi.HttpClients;
using PhotosiApi.Settings;

namespace PhotosiApi.Service.Product;

public class ProductService : IProductService
{
    private readonly IBaseHttpClient _baseHttpClient;

    private readonly string _photosiProducstUrl;

    public ProductService(IOptions<AppSettings> options, IBaseHttpClient baseHttpClient)
    {
        _photosiProducstUrl = options.Value.PhotosiProductsUrl;
        _baseHttpClient = baseHttpClient;
    }

    public async Task<List<ProductDto>> GetAsync()
    {
        var products = await _baseHttpClient.Get<List<ProductDto>>(_photosiProducstUrl);
        if (products == null)
            throw new ProductException("Prodotti non trovati");

        return products;
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await _baseHttpClient.Get<ProductDto>($"{_photosiProducstUrl}/{id}");
        if (product == null)
            throw new ProductException("Prodotto non trovato");

        return product;
    }

    public async Task<bool> UpdateAsync(int id, ProductDto productRequest) =>
        await _baseHttpClient.Put($"{_photosiProducstUrl}/{id}", productRequest);

    public async Task<ProductDto> AddAsync(ProductDto productRequest)
    {
        var newProduct = await _baseHttpClient.Post<ProductDto>(_photosiProducstUrl, productRequest);
        if (newProduct == null)
            throw new ProductException("Prodotto nullo nella risposta");

        return newProduct;
    }

    public async Task<bool> DeleteAsync(int id) =>
        await _baseHttpClient.Delete($"{_photosiProducstUrl}/{id}");
}