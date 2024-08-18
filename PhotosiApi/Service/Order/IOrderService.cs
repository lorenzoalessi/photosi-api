using PhotosiApi.Dto.Order;

namespace PhotosiApi.Service.Order;

public interface IOrderService
{
    Task<EntireOrderDto> GetByIdAsync(int id);
    
    Task<OrderDto> AddAsync(OrderDto orderRequest);
}