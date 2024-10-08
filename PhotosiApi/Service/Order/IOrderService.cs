﻿using PhotosiApi.Dto.Order;

namespace PhotosiApi.Service.Order;

public interface IOrderService
{
    Task<List<EntireOrderDto>> GetAllForUser(int userId);
    
    Task<EntireOrderDto> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, OrderDto orderRequest);
    
    Task<OrderDto> AddAsync(OrderDto orderRequest);

    Task<bool> DeleteAsync(int id);
}