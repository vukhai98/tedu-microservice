﻿using Shared.DTOs.Orders;

namespace Saga.Orchestrator.HttpRepository.Interfaces
{
    public interface IOrderHttpRepository
    {
        Task<long> CreateOrder(CreateOrderDto orderDto);
        Task<OrderDto> GetOrder(long  id);
        Task<bool> DeleteOrder(long  id);
        Task<bool> DeleteOrderByDocumentNo(string  documentNo);

        

    }
}
