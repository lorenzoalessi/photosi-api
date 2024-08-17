using PhotosiApi.Dto.AddressBook;

namespace PhotosiApi.Service.AddressBook;

public interface IAddressBookService
{
    Task<List<AddressBookDto>> GetAsync();
    
    Task<AddressBookDto> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, AddressBookDto addressBookRequest);

    Task<AddressBookDto> AddAsync(AddressBookDto addressBookRequest);
    
    Task<bool> DeleteAsync(int id);
}