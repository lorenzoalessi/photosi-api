using Microsoft.AspNetCore.Mvc;
using PhotosiApi.Dto.AddressBook;
using PhotosiApi.Exceptions;
using PhotosiApi.Security;
using PhotosiApi.Service.AddressBook;

namespace PhotosiApi.Controllers;

[Route("api/v1/address-books")]
[ApiController]
public class AddressBookController : BaseController
{
    private readonly IAddressBookService _addressBookService;

    public AddressBookController(IAddressBookService addressBookService)
    {
        _addressBookService = addressBookService;
    }
    
    [HttpGet]
    [ValidToken]
    public async Task<IActionResult> Get()
    {
        try
        {
            return Ok(await _addressBookService.GetAsync());
        }
        catch (AddressBookException e)
        {
            return StatusCode(500, $"Errore nel recupero degli indirizzi: {e.Message}");
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
            return Ok(await _addressBookService.GetByIdAsync(id));
        }
        catch (AddressBookException e)
        {
            return StatusCode(500, $"Errore nel recupero dell'indirizzo: {e.Message}");
        }
    }
    
    [HttpPut("{id}")]
    [ValidToken]
    public async Task<IActionResult> Update(int id, [FromBody] AddressBookDto addressBookRequest)
    {
        if (id < 1)
            return BadRequest("ID fornito non valido");

        var updated = await _addressBookService.UpdateAsync(id, addressBookRequest);
        if (!updated)
            return StatusCode(500, "Qualcosa è andato storto, indirizzo non modificato");
            
        return Ok("Indirizzo aggiornato con successo");
    }
    
    [HttpPost]
    [ValidToken]
    public async Task<IActionResult> Add([FromBody] AddressBookDto addressBookRequest)
    {
        try
        {
            return Ok(await _addressBookService.AddAsync(addressBookRequest));
        }
        catch (AddressBookException e)
        {
            return StatusCode(500, $"Errore nell'inserimento dell'indirizzo: {e.Message}");
        }
    }

    [HttpDelete("{id}")]
    [ValidToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (id < 1)
            return BadRequest("ID fornito non valido");

        var deleted = await _addressBookService.DeleteAsync(id);
        if (!deleted)
            return StatusCode(500, "Qualcosa è andato storto, indirizzo non eliminato");
        
        return Ok("Indirizzo eliminato con successo");
    }
}