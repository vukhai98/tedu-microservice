using Basket.API.Repositories.Interfaces;
using Basket.API.Repositories;
using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Infrastructure.Configurations;
using Shared.Configurations;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MassTransit;
using EventBus.Messages.IntegrationEvents.Interfaces;

namespace Basket.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ISerializeService, SerializeService>()
                    .AddScoped<IBasketRepository, BasketRepository>();

            return services;
        }

        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();

            services.AddSingleton(eventBusSettings);

            var cacheSettings = configuration.GetSection(nameof(CacheSettings)).Get<CacheSettings>();

            services.AddSingleton(cacheSettings);

            return services;
        }

        public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheSettings = services.GetOptions<CacheSettings>("CacheSettings");

            if (string.IsNullOrEmpty(cacheSettings.ConnectionString))
                throw new ArgumentNullException("Redis connection string is not configured.");

            services.AddStackExchangeRedisCache(x =>
            {
                x.Configuration = cacheSettings.ConnectionString;
            });
        }

        public static void ConfigueMassTransit(this IServiceCollection services)
        {
            var settings = services.GetOptions<EventBusSettings>("EventBusSettings");
            if (settings == null || string.IsNullOrEmpty(settings.HostAddress))
                throw new ArgumentNullException("EventBusSettings is not configured.");

            var mqConnection = new Uri(settings.HostAddress);

            // Convert "BasketCheckoutEventQueue" to "basket-checkout-event-queue"
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                });

                // Publish submmit order message
                config.AddRequestClient<IBasketCheckoutEvent>();
            });
        }
    }
}
