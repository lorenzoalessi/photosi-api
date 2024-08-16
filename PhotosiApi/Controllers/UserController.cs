using Microsoft.AspNetCore.Mvc;
using PhotosiApi.Dto;
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
    public async Task<IActionResult> RegisterAsync(UserDto userDto)
    {
        return null;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
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