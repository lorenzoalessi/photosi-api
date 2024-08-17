using Microsoft.AspNetCore.Mvc;
using PhotosiApi.Dto;
using PhotosiApi.Exceptions;
using PhotosiApi.Security;
using PhotosiApi.Service.User;

namespace PhotosiApi.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto userRequest)
    {
        try
        {
            var newUser = await _userService.RegisterAsync(userRequest);
            return Ok($"Utente creato con successo! ID: {newUser.Id}");
        }
        catch (Exception e) when (e is UserException or BaseHttpClientException)
        {
            return StatusCode(500, $"Errore nella registrazione: {e.Message}");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Username))
            // Per questioni di sicurezza torno sempre un 500 con messaggio generico
            return StatusCode(500, "Errore nella richiesta");

        try
        {
            return Ok(await _userService.LoginAsync(loginDto));
        }
        catch (Exception)
        {
            // Per questioni di sicurezza torno sempre un 500 con messaggio generico
            return StatusCode(500, "Errore nella richiesta");
        }
    }

    [HttpGet("ping")]
    [ValidToken]
    public IActionResult Ping()
    {
        return Ok();
    }
}