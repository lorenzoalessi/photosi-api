using System.Diagnostics.CodeAnalysis;

namespace PhotosiApi.Dto.Order;

[ExcludeFromCodeCoverage]
public class OrderProductDto
{
    public int Id { get; set; }
    
    public int Quantity { get; set; }
}