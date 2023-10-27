using Saga.Orchestrator.HttpRepository.Interfaces;
using Shared.DTOs.Baskets;

namespace Saga.Orchestrator.HttpRepository
{
    public class BasketHttpRepository : IBasketHttpRepository
    {
        private readonly HttpClient _httpClient;

        public BasketHttpRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<bool> DeleteBasket(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<CartDTO> GetBasket(string userName)
        {
            var cart = await _httpClient.GetFromJsonAsync<CartDTO>($"baskets/{userName}");

            if (cart == null || !cart.Items.Any())
                return null;

            return cart;
        }
    }
}
