using Microsoft.Extensions.Options;
using PhotosiApi.Dto.User;
using PhotosiApi.Exceptions;
using PhotosiApi.HttpClients;
using PhotosiApi.Service.User.Login;
using PhotosiApi.Settings;
using PhotosiApi.Utility;

namespace PhotosiApi.Service.User;

public class UserService : IUserService
{
    private readonly IUserLoginHandler _userLoginHandler;
    private readonly IBaseHttpClient _baseHttpClient;

    private readonly string _photosiUsersUrl;
    
    public UserService(IOptions<AppSettings> options, IUserLoginHandler userLoginHandler, IBaseHttpClient baseHttpClient)
    {
         _photosiUsersUrl = options.Value.PhotosiUsersUrl;
         _userLoginHandler = userLoginHandler;
        _baseHttpClient = baseHttpClient;
    }

    public async Task<UserDto> RegisterAsync(UserDto userRequest)
    {
        var newUser = await _baseHttpClient.Post<UserDto>($"{_photosiUsersUrl}", userRequest);
        if (newUser == null)
            throw new UserException("Utente nullo in risposta");

        return newUser;
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var userDto = await _baseHttpClient.Post<UserDto>($"{_photosiUsersUrl}/login", loginDto);
        if (userDto == null)
            throw new UserException("Utente non trovato");

        var token = JwtUtils.GenerateJwtToken(userDto);
        
        // Aggiungo l'utente tra quelli loggati
        _userLoginHandler.Users.Add(new LoggedUser()
        {
            User = userDto,
            Token = token
        });

        return token;
    }

    public async Task<List<UserDto>> GetAsync()
    {
        var users = await _baseHttpClient.Get<List<UserDto>>(_photosiUsersUrl);
        if (users == null)
            throw new UserException("Utenti non trovati");

        return users;
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _baseHttpClient.Get<UserDto>($"{_photosiUsersUrl}/{id}");
        if (user == null)
            throw new UserException("Utente non trovato");

        return user;
    }

    public async Task<bool> UpdateAsync(int id, UserDto userRequest) => 
        await _baseHttpClient.Put($"{_photosiUsersUrl}/{id}", userRequest);

    public async Task<bool> DeleteAsync(int id) => 
        await _baseHttpClient.Delete($"{_photosiUsersUrl}/{id}");
}