using System.Diagnostics.CodeAnalysis;

namespace PhotosiApi.Dto.Product;

[ExcludeFromCodeCoverage]
public class EntireProductDto : ProductDto
{
    public int Quantity { get; set; }
}