using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using ILogger = Serilog.ILogger;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly ISerializeService _serializeService;

        private readonly IDistributedCache _redisCacheService;
        private readonly ILogger<BasketRepository> _logger;

        public BasketRepository(ISerializeService serializeService, IDistributedCache redisCacheService, ILogger<BasketRepository> logger)
        {
            _serializeService = serializeService;
            _redisCacheService = redisCacheService;
            _logger = logger;
        }

        public async Task<bool> DeleteBasketFromUserName(string userName)
        {
            try
            {
                await _redisCacheService.RemoveAsync(userName);

                return true;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return false;
            }
          
        }

        public async Task<Cart?> GetBasketByUserName(string userName)
        {
            var basket = await _redisCacheService.GetStringAsync(userName);

            return string.IsNullOrEmpty(basket) ? null : _serializeService.Deserialize<Cart>(basket);
        }

        public async Task<Cart> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null)
        {
            var key = cart.UserName;
            var value = _serializeService.Serialize(cart);

            if (options != null)
            {
                await _redisCacheService.SetStringAsync(key, value, options);
            }
            else
            {
                await _redisCacheService.SetStringAsync(key, value);
            }

            var result = await GetBasketByUserName(key);

            return result;
        }
    }
}
