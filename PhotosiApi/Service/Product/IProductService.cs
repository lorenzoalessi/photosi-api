using PhotosiApi.Dto.Product;

namespace PhotosiApi.Service.Product;

public interface IProductService
{
    Task<List<ProductDto>> GetAsync();
    
    Task<ProductDto> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, ProductDto productRequest);

    Task<ProductDto> AddAsync(int id, ProductDto productRequest);
    
    Task<bool> DeleteAsync(int id);
}