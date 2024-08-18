using System.Diagnostics.CodeAnalysis;

namespace PhotosiApi.Dto.AddressBook;

[ExcludeFromCodeCoverage]
public class AddressBookDto
{
    public int Id { get; set; }

    public string AddressName { get; set; }

    public string AddressNumber { get; set; }

    public string Cap { get; set; }

    public string CityName { get; set; }

    public string CountryName { get; set; }
}