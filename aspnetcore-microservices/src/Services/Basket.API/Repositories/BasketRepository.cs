using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Basket.API.Services.Interfaces;
using Basket.API.Services;
using Contracts.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using ILogger = Serilog.ILogger;
using Shared.DTOs.ScheduledJob;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly ISerializeService _serializeService;

        private readonly IDistributedCache _redisCacheService;
        private readonly ILogger<BasketRepository> _logger;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly BackgroundJobHttpService _backgroundJobHttpService;
        public BasketRepository(ISerializeService serializeService, IDistributedCache redisCacheService, ILogger<BasketRepository> logger, IEmailTemplateService emailTemplateService, BackgroundJobHttpService backgroundJobHttpService)
        {
            _serializeService = serializeService;
            _redisCacheService = redisCacheService;
            _logger = logger;
            _emailTemplateService = emailTemplateService;
            _backgroundJobHttpService = backgroundJobHttpService;
        }

        public async Task<bool> DeleteBasketFromUserName(string userName)
        {
            try
            {
                _logger.LogInformation($"BEGIN: GetBasketByUserName {userName}");

                await _redisCacheService.RemoveAsync(userName);
                _logger.LogInformation($"BEGIN: GetBasketByUserName {userName}");


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
            _logger.LogInformation($"BEGIN: GetBasketByUserName {userName}");

            var basket = await _redisCacheService.GetStringAsync(userName);

            _logger.LogInformation($"END: GetBasketByUserName {userName}");


            return string.IsNullOrEmpty(basket) ? null : _serializeService.Deserialize<Cart>(basket);
        }

        public async Task<Cart> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null)
        {
            _logger.LogInformation($"BEGIN: Update Basket for {cart.UserName}");

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

            _logger.LogInformation($"END: Update Basket for {cart.UserName}");

            try
            {

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }

            var result = await GetBasketByUserName(key);

            return result;
        }

        private async Task TriggerSendEmailReminderCheckout(Cart cart)
        {
            var emailTemplate = _emailTemplateService.GenerateReminderCheckoutOrderEmail(cart.UserName);

            var model = new ReminderCheckoutOrderDto()
            {
                enqueueAt = DateTime.UtcNow.AddSeconds(30),
                Email = cart.EmailAddress,
                EmailContent = emailTemplate,
                Subject= "Reminder check out"
            };


        }
    }
}
