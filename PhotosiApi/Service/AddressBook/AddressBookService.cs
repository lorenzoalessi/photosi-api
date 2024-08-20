using Microsoft.Extensions.Options;
using PhotosiApi.Dto.AddressBook;
using PhotosiApi.Exceptions;
using PhotosiApi.HttpClients;
using PhotosiApi.Settings;

namespace PhotosiApi.Service.AddressBook;

public class AddressBookService : IAddressBookService
{
    private readonly IBaseHttpClient _baseHttpClient;

    private readonly string _photosiAddressBooksUrl;

    public AddressBookService(IOptions<AppSettings> options, IBaseHttpClient baseHttpClient)
    {
        _photosiAddressBooksUrl = options.Value.PhotosiAddressBooksUrl;
        _baseHttpClient = baseHttpClient;
    }
    
    public async Task<List<AddressBookDto>> GetAsync()
    {
        var addressBooks = await _baseHttpClient.Get<List<AddressBookDto>>(_photosiAddressBooksUrl);
        if (addressBooks == null)
            throw new AddressBookException("Indirizzi non trovati");

        return addressBooks;
    }

    public async Task<AddressBookDto> GetByIdAsync(int id)
    {
        var addressBook = await _baseHttpClient.Get<AddressBookDto>($"{_photosiAddressBooksUrl}/{id}");
        if (addressBook == null)
            throw new AddressBookException("Indirizzo non trovato");

        return addressBook;
    }

    public async Task<bool> UpdateAsync(int id, AddressBookDto addressBookRequest) =>
        await _baseHttpClient.Put($"{_photosiAddressBooksUrl}/{id}", addressBookRequest);

    public async Task<AddressBookDto> AddAsync(AddressBookDto addressBookRequest)
    {
        var newAddressBook =
            await _baseHttpClient.Post<AddressBookDto>(_photosiAddressBooksUrl, addressBookRequest);
        if (newAddressBook == null)
            throw new AddressBookException("Indirizzo nullo nella risposta");

        return newAddressBook;
    }

    public async Task<bool> DeleteAsync(int id) =>
        await _baseHttpClient.Delete($"{_photosiAddressBooksUrl}/{id}");
}