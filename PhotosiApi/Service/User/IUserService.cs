using PhotosiApi.Dto;

namespace PhotosiApi.Service.User;

public interface IUserService
{
    Task<UserDto> RegisterAsync(UserDto userRequest);

    Task<string> LoginAsync(LoginDto loginDto);
}