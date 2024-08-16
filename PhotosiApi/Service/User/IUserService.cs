using PhotosiApi.Dto;

namespace PhotosiApi.Service.User;

public interface IUserService
{
    Task<string> LoginAsync(LoginDto loginDto);
}