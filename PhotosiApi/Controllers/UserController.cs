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

    [HttpGet]
    [ValidToken]
    public async Task<IActionResult> Get()
    {
        try
        {
            return Ok(await _userService.GetAsync());
        }
        catch (UserException e)
        {
            return StatusCode(500, $"Errore nel recupero degli utenti: {e.Message}");
        }
    }
    
    [HttpGet("{id}")]
    [ValidToken]
    public async Task<IActionResult> GetById(int id)
    {
        if (id < 1)
            return BadRequest("ID fornito non valido");
        
        try
        {
            return Ok(await _userService.GetByIdAsync(id));
        }
        catch (UserException e)
        {
            return StatusCode(500, $"Errore nel recupero degli utenti: {e.Message}");
        }
    }

    [HttpPut("{id}")]
    [ValidToken]
    public async Task<IActionResult> Update(int id, [FromBody] UserDto userRequest)
    {
        if (id < 1)
            return BadRequest("ID fornito non valido");

        var updated = await _userService.UpdateAsync(id, userRequest);
        if (!updated)
            return StatusCode(500, "Qualcosa è andato storto, utente non modificato");
            
        return Ok("Utente aggiornato con successo");
    }

    [HttpDelete("{id}")]
    [ValidToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (id < 1)
            return BadRequest("ID fornito non valido");

        var deleted = await _userService.DeleteAsync(id);
        if (!deleted)
            return StatusCode(500, "Qualcosa è andato storto, utente non eliminato");
        
        return Ok("Utente eliminato con successo");
    }

    [HttpGet("ping")]
    [ValidToken]
    public IActionResult Ping()
    {
        return Ok();
    }
}