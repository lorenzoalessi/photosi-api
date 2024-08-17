using PhotosiApi.Dto;

namespace PhotosiApi.Service.User;

public interface IUserService
{
    Task<UserDto> RegisterAsync(UserDto userRequest);

    Task<string> LoginAsync(LoginDto loginDto);

    Task<List<UserDto>> GetAsync();
    
    Task<UserDto> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, UserDto userRequest);
    
    Task<bool> DeleteAsync(int id);
}