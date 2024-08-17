using Microsoft.Extensions.Options;
using PhotosiApi.Dto;
using PhotosiApi.Exceptions;
using PhotosiApi.HttpClients.User;
using PhotosiApi.Settings;
using PhotosiApi.Utility;

namespace PhotosiApi.Service.User;

public class UserService : IUserService
{
    private readonly IUserLoginHandler _userLoginHandler;
    private readonly IUserHttpClient _userHttpClient;

    private readonly string _photosiUsersUrl;
    
    public UserService(IOptions<AppSettings> options, IUserLoginHandler userLoginHandler, IUserHttpClient userHttpClient)
    {
         _photosiUsersUrl = options.Value.PhotosiUsersUrl;
         _userLoginHandler = userLoginHandler;
        _userHttpClient = userHttpClient;
    }

    public async Task<UserDto> RegisterAsync(UserDto userRequest)
    {
        var newUser = await _userHttpClient.Post<UserDto>($"{_photosiUsersUrl}", userRequest);
        if (newUser == null)
            throw new UserException("Utente nullo in risposta");

        return newUser;
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var userDto = await _userHttpClient.Post<UserDto>($"{_photosiUsersUrl}/login", loginDto);
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
}