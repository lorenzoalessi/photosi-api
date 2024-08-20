using Microsoft.Extensions.Options;
using PhotosiApi.Dto.AddressBook;
using PhotosiApi.Dto.Order;
using PhotosiApi.Dto.Product;
using PhotosiApi.Exceptions;
using PhotosiApi.HttpClients;
using PhotosiApi.Settings;

namespace PhotosiApi.Service.Order;

public class OrderService : IOrderService
{
    private readonly IBaseHttpClient _baseHttpClient;

    private readonly string _photosiOrdersUrl;
    private readonly string _photosiProductsUrl;
    private readonly string _photosiAddressBooksUrl;
    
    public OrderService(IOptions<AppSettings> options, IBaseHttpClient baseHttpClient)
    {
        _photosiOrdersUrl = options.Value.PhotosiOrdersUrl;
        _photosiProductsUrl = options.Value.PhotosiProductsUrl;
        _photosiAddressBooksUrl = options.Value.PhotosiAddressBooksUrl;

        _baseHttpClient = baseHttpClient;
    }

    public async Task<List<EntireOrderDto>> GetAllForUser(int userId)
    {
        // Recupero gli ordini
        var orders = await _baseHttpClient.Get<List<OrderDto>>($"{_photosiOrdersUrl}/user/{userId}");
        if (orders == null)
            throw new OrderException("Ordini non trovati");

        if (orders.Count == 0)
            return [];

        var ordersTask = orders.Select(async order =>
        {
            // Recupero tutti i prodotti (per ogni ordine)
            var productsTask = order.OrderProducts.Select(async product =>
            {
                var entireProductDto = await _baseHttpClient.Get<EntireProductDto>($"{_photosiProductsUrl}/{product.Id}");
                if (entireProductDto == null)
                    throw new OrderException($"Prodotto con ID {product.Id} non trovato");

                entireProductDto.Quantity = product.Quantity;
                return entireProductDto;
            });

            var entireProductsList = (await Task.WhenAll(productsTask)).ToList();
            
            // Recupero l'indirizzo (per ogni ordine)
            var addressBook = await AddressBookExist(order.AddressId);
            
            return new EntireOrderDto()
            {
                Id = order.Id,
                OrderCode = order.OrderCode,
                UserId = userId,
                AddressBookDto = addressBook,
                EntireProductDtos = entireProductsList
            };
        });

        return (await Task.WhenAll(ordersTask)).ToList();
    }

    public async Task<EntireOrderDto> GetByIdAsync(int id)
    {
        // Recupero l'ordine
        var orderDto = await _baseHttpClient.Get<OrderDto>($"{_photosiOrdersUrl}/{id}");
        if (orderDto == null)
            throw new OrderException("Ordine non trovato");

        // Recupero i prodotti
        var entireProductDtos = new List<EntireProductDto>();
        var productsTask = orderDto.OrderProducts.Select(async x =>
        {
            var entireProductDto = await _baseHttpClient.Get<EntireProductDto>($"{_photosiProductsUrl}/{x.Id}");
            if (entireProductDto == null)
                throw new OrderException($"Prodotto con ID {x.Id} non trovato");

            entireProductDto.Quantity = x.Quantity;
            entireProductDtos.Add(entireProductDto);
        });
        
        await Task.WhenAll(productsTask);
        
        // Recupero l'indirizzo
        var addressBookDto = await AddressBookExist(orderDto.AddressId);

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

    public async Task<bool> UpdateAsync(int id, OrderDto orderRequest)
    {
        var order = await _baseHttpClient.Get<OrderDto>($"{_photosiOrdersUrl}/{id}");
        if (order == null)
            throw new OrderException("Ordine non trovato");
        
        // Controllo che i prodotti esistano
        await ProductsExists(orderRequest.OrderProducts);
        
        // Controllo l'indirizzo
        _ = await AddressBookExist(orderRequest.AddressId);
        
        // Modifico l'ordine
        return await _baseHttpClient.Put($"{_photosiOrdersUrl}/{id}", orderRequest);
    }

    public async Task<OrderDto> AddAsync(OrderDto orderRequest)
    {
        // Controllo che i prodotti esistano
        await ProductsExists(orderRequest.OrderProducts);
        
        // Controllo l'indirizzo
        _ = await AddressBookExist(orderRequest.AddressId);
        
        // Aggiungo l'ordine
        var newOrder = await _baseHttpClient.Post<OrderDto>(_photosiOrdersUrl, orderRequest);
        if (newOrder == null)
            throw new OrderException("Ordine nullo nella risposta");
        
        return newOrder;
    }

    public async Task<bool> DeleteAsync(int id) =>
        await _baseHttpClient.Delete($"{_photosiOrdersUrl}/{id}");

    private async Task ProductsExists(List<OrderProductDto> orderProductDtos)
    {
        // Controllo i prodotti
        var productsTask = orderProductDtos.Select(async x =>
        {
            var productDto = await _baseHttpClient.Get<ProductDto>($"{_photosiProductsUrl}/{x.Id}");
            if (productDto == null)
                throw new OrderException($"Prodotto con ID {x.Id} non trovato");
        });

        // N.B. -> Il metodo .ForEach() sulla lista non mi permette di effettuare le chiamate in modo asyncrono quindi
        // opto per un Task.WhenAll()
        // Attendo il completamento delle chiamate asyncrone per il controllo dei prodotti
        await Task.WhenAll(productsTask);
    }
    
    private async Task<AddressBookDto> AddressBookExist(int addressBookId)
    {
        var addressBookDto =
            await _baseHttpClient.Get<AddressBookDto>($"{_photosiAddressBooksUrl}/{addressBookId}");
        if (addressBookDto == null)
            throw new OrderException($"Indirizzo con ID {addressBookId} non trovato");

        return addressBookDto;
    }
}