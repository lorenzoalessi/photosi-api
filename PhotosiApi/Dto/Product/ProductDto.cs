using System.Diagnostics.CodeAnalysis;

namespace PhotosiApi.Dto.Product;

[ExcludeFromCodeCoverage]
public class ProductDto
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}