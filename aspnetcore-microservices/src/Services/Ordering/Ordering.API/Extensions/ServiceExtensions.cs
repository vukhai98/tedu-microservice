using EventBus.Messages.IntegrationEvents.Events;
using EventBus.Messages.IntegrationEvents.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.API.Application.IntegrationEvents.EventsHandler;
using Shared.Configurations;

namespace Ordering.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection(nameof(SmtpEmailSettings)).Get<SmtpEmailSettings>();

            services.AddSingleton(emailSettings);

            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();

            services.AddSingleton(eventBusSettings);

            return services;
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
                config.AddConsumersFromNamespaceContaining<BasketCheckoutEventHandler>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);

                    // Cấu hình cho 1 consummer cụ thể
                    //cfg.ReceiveEndpoint("basket-check-out-queue", c =>
                    //{
                    //    c.ConfigureConsumer<BasketCheckoutEventHandler>(ctx);
                    //});

                    // Tất cả thằng nào Implement IConsummer sẽ được RabbitMQ nhận! 
                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }
    }
}
