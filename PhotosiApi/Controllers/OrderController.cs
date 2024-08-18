using Microsoft.AspNetCore.Mvc;
using PhotosiApi.Dto.Order;
using PhotosiApi.Exceptions;
using PhotosiApi.Security;
using PhotosiApi.Service.Order;

namespace PhotosiApi.Controllers;

[Route("api/v1/orders")]
[ApiController]
public class OrderController : BaseController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{id}")]
    [ValidToken]
    public async Task<IActionResult> GetById(int id)
    {
        if (id < 1)
            return BadRequest("ID fornito non valido");

        try
        {
            return Ok(await _orderService.GetByIdAsync(id));
        }
        catch (OrderException e)
        {
            return StatusCode(500, $"Errore nel recupero dell'ordine: {e.Message}");
        }
    }

    [HttpPost]
    [ValidToken]
    public async Task<IActionResult> Add([FromBody] OrderDto orderRequest)
    {
        if (orderRequest.OrderProducts.Count == 0)
            return BadRequest("Nessun prodotto associato all'ordine");
        
        if (orderRequest.AddressId < 1)
            return BadRequest("ID indirizzo fornito non valido");

        var userLoggedId = LoggedUser?.User.Id ?? 0;
        if (userLoggedId < 1)
            return BadRequest("Errore nella sessione dell'utente, ritentare il login");
        
        try
        {
            // ID dell'utente loggato
            orderRequest.UserId = userLoggedId;
            return Ok(await _orderService.AddAsync(orderRequest));
        }
        catch (OrderException e)
        {
            return StatusCode(500, $"Errore nell'inserimento dell'ordine: {e.Message}");
        }
    }
}