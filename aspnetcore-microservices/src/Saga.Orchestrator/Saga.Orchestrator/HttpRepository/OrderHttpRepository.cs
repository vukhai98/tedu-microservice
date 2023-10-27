using Infrastructure.Extensions;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Shared.DTOs.Orders;
using Shared.SeedWork;

namespace Saga.Orchestrator.HttpRepository
{
    public class OrderHttpRepository : IOrderHttpRepository
    {
        private readonly HttpClient _httpClient;
        public OrderHttpRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient = httpClient;
        }

        public async Task<long> CreateOrder(CreateOrderDto orderDto)
        {
            var respone = await _httpClient.PostAsJsonAsync("order", orderDto);

            if (!respone.EnsureSuccessStatusCode().IsSuccessStatusCode)
                return -1;
            var result = await respone.ReadContentAs<ApiSuccessedResult<long>>();

            return result.Data;
        }

        public async Task<bool> DeleteOrder(long id)
        {
            var result = await _httpClient.DeleteAsync($"order/{id.ToString()}");

            return result.EnsureSuccessStatusCode().IsSuccessStatusCode;

        }

        public async Task<bool> DeleteOrderByDocumentNo(string documentNo)
        {
            var result = await _httpClient.DeleteAsync($"order/document-no/{documentNo}");

            return result.EnsureSuccessStatusCode().IsSuccessStatusCode;

        }

        public async Task<OrderDto> GetOrder(long id)
        {
            var result = await _httpClient.GetFromJsonAsync<ApiSuccessedResult<OrderDto>>($"order/{id.ToString()}");

            return result.Data;
        }
    }
}
