using System.Diagnostics.CodeAnalysis;
using PhotosiApi.Dto.AddressBook;
using PhotosiApi.Dto.Product;

namespace PhotosiApi.Dto.Order;

[ExcludeFromCodeCoverage]
public class EntireOrderDto
{
    public int Id { get; set; }
    
    public int OrderCode { get; set; }
    
    public int UserId { get; set; }
    
    public AddressBookDto AddressBookDto { get; set; }
    
    public List<EntireProductDto> EntireProductDtos { get; set; }
}