using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Basket.API.Services.Interfaces;
using Basket.API.Services;
using Contracts.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using ILogger = Serilog.ILogger;
using Shared.DTOs.ScheduledJob;
using Infrastructure.Extensions;

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
            DeleteReminderCheckoutOrder(userName);

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
            DeleteReminderCheckoutOrder(cart.UserName);

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
                await TriggerSendEmailReminderCheckout(cart);
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
                enqueueAt = DateTime.Now.AddSeconds(30),
                Email = cart.EmailAddress,
                EmailContent = emailTemplate,
                Subject = "Reminder check out"
            };

           var uri = $"{_backgroundJobHttpService.ScheduledJobUrl}/send-mail-reminder-checkout-order";

            var respone = await _backgroundJobHttpService.Client.PostAsJsonAsync(uri, model);

            if (respone.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                var jobId = await respone.ReadContentAs<string>();

                if (!string.IsNullOrEmpty(jobId))
                {
                    cart.JobId = jobId;
                    await _redisCacheService.SetStringAsync(cart.UserName, _serializeService.Serialize(cart));
                }
            }

        }

        private async Task DeleteReminderCheckoutOrder(string userName)
        {
            var cart = await GetBasketByUserName(userName);

            if (cart == null || !string.IsNullOrEmpty(cart.JobId))
                return;

            var uri = $"{_backgroundJobHttpService.ScheduledJobUrl}/delete/{cart.JobId}";

            _backgroundJobHttpService.Client.DeleteAsync(uri);

            _logger.LogInformation($"Delete ReminderCheckoutOrder: Deleted JobId: {cart.JobId}");
        }
    }
}
