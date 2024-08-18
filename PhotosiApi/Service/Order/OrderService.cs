using Microsoft.Extensions.Options;
using PhotosiApi.Dto.AddressBook;
using PhotosiApi.Dto.Order;
using PhotosiApi.Dto.Product;
using PhotosiApi.Exceptions;
using PhotosiApi.HttpClients.AddressBook;
using PhotosiApi.HttpClients.Order;
using PhotosiApi.HttpClients.Product;
using PhotosiApi.Settings;

namespace PhotosiApi.Service.Order;

public class OrderService : IOrderService
{
    private readonly IOrderHttpClient _orderHttpClient;
    private readonly IProductHttpClient _productHttpClient;
    private readonly IAddressBookHttpClient _addressBookHttpClient;

    private readonly string _photosiOrdersUrl;
    private readonly string _photosiProductsUrl;
    private readonly string _photosiAddressBooksUrl;
    
    public OrderService(IOptions<AppSettings> options,
        IOrderHttpClient orderHttpClient, 
        IProductHttpClient productHttpClient, 
        IAddressBookHttpClient addressBookHttpClient)
    {
        _photosiOrdersUrl = options.Value.PhotosiOrdersUrl;
        _photosiProductsUrl = options.Value.PhotosiProductsUrl;
        _photosiAddressBooksUrl = options.Value.PhotosiAddressBooksUrl;
        
        _orderHttpClient = orderHttpClient;
        _productHttpClient = productHttpClient;
        _addressBookHttpClient = addressBookHttpClient;
    }

    public async Task<EntireOrderDto> GetByIdAsync(int id)
    {
        // Recupero l'ordine
        var orderDto = await _orderHttpClient.Get<OrderDto>($"{_photosiOrdersUrl}/{id}");
        if (orderDto == null)
            throw new OrderException("Ordine non trovato");

        // Recupero i prodotti
        var entireProductDtos = new List<EntireProductDto>();
        var productsTask = orderDto.OrderProducts.Select(async x =>
        {
            var entireProductDto = await _productHttpClient.Get<EntireProductDto>($"{_photosiProductsUrl}/{x.Id}");
            if (entireProductDto == null)
                throw new OrderException($"Prodotto con ID {x.Id} non trovato");

            entireProductDto.Quantity = x.Quantity;
            entireProductDtos.Add(entireProductDto);
        });
        
        await Task.WhenAll(productsTask);
        
        // Recupero l'indirizzo
        var addressBookDto =
            await _addressBookHttpClient.Get<AddressBookDto>($"{_photosiAddressBooksUrl}/{orderDto.AddressId}");
        if (addressBookDto == null)
            throw new OrderException($"Indirizzo con ID {orderDto.Id} non trovato");

        // Torno la dto con tutti i dati
        return new EntireOrderDto()
        {
            Id = orderDto.Id,
            OrderCode = orderDto.OrderCode,
            UserId = orderDto.UserId,
            AddressBookDto = addressBookDto,
            EntireProductDtos = entireProductDtos
        };
    }

    public async Task<OrderDto> AddAsync(OrderDto orderRequest)
    {
        // Controllo i prodotti
        var productsTask = orderRequest.OrderProducts.Select(async x =>
        {
            var productDto = await _productHttpClient.Get<ProductDto>($"{_photosiProductsUrl}/{x.Id}");
            if (productDto == null)
                throw new OrderException($"Prodotto con ID {x.Id} non trovato");
        });

        // N.B. -> Il metodo .ForEach() sulla lista non mi permette di effettuare le chiamate in modo asyncrono quindi
        // opto per un Task.WhenAll()
        // Attendo il completamento delle chiamate asyncrone per il controllo dei prodotti
        await Task.WhenAll(productsTask);
        
        // Controllo l'indirizzo
        var addressBookDto =
            await _addressBookHttpClient.Get<AddressBookDto>($"{_photosiAddressBooksUrl}/{orderRequest.AddressId}");
        if (addressBookDto == null)
            throw new OrderException($"Indirizzo con ID {orderRequest.Id} non trovato");
        
        // Aggiungo l'ordine
        var newOrder = await _orderHttpClient.Post<OrderDto>(_photosiOrdersUrl, orderRequest);
        if (newOrder == null)
            throw new OrderException("Ordine nullo nella risposta");
        
        return newOrder;
    }
}