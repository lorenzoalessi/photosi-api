using Microsoft.AspNetCore.Mvc;
using PhotosiApi.Dto;
using PhotosiApi.Dto.Product;
using PhotosiApi.Exceptions;
using PhotosiApi.Security;
using PhotosiApi.Service.Product;

namespace PhotosiApi.Controllers;

[Route("api/v1/users")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [ValidToken]
    public async Task<IActionResult> Get()
    {
        try
        {
            return Ok(await _productService.GetAsync());
        }
        catch (UserException e)
        {
            return StatusCode(500, $"Errore nel recupero dei prodotti: {e.Message}");
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
            return Ok(await _productService.GetByIdAsync(id));
        }
        catch (UserException e)
        {
            return StatusCode(500, $"Errore nel recupero del prodotto: {e.Message}");
        }
    }

    [HttpPut("{id}")]
    [ValidToken]
    public async Task<IActionResult> Update(int id, [FromBody] ProductDto productRequest)
    {
        if (id < 1)
            return BadRequest("ID fornito non valido");

        var updated = await _productService.UpdateAsync(id, productRequest);
        if (!updated)
            return StatusCode(500, "Qualcosa è andato storto, prodotto non modificato");
            
        return Ok("Prodotto aggiornato con successo");
    }

    [HttpDelete("{id}")]
    [ValidToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (id < 1)
            return BadRequest("ID fornito non valido");

        var deleted = await _productService.DeleteAsync(id);
        if (!deleted)
            return StatusCode(500, "Qualcosa è andato storto, prodotto non eliminato");
        
        return Ok("Prodotto eliminato con successo");
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok();
    }
}