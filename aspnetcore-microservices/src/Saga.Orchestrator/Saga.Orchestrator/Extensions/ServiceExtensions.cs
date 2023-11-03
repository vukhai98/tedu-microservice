using Saga.Orchestrator.HttpRepository;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Saga.Orchestrator.Services;
using Saga.Orchestrator.Services.Interfaces;

namespace Saga.Orchestrator.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IBasketHttpRepository, BasketHttpRepository>()
                    .AddTransient<IOrderHttpRepository, OrderHttpRepository>()
                    .AddTransient<IInventoryHttpRepository, InventoryHttpRepository>()
                    .AddScoped<ICheckoutService, CheckoutService>();

            return services;
        }

        public static void ConfigureHttpClients(this IServiceCollection services)
        {
            ConfigureOrderHttpClient(services);
            ConfigureIventoryHttpClient(services);
            ConfigureBasketHttpClient(services);
        }

        public static void ConfigureOrderHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IOrderHttpRepository, OrderHttpRepository>("OrderAPI", (_serviceProvider, httpClient) =>
            {
                httpClient.BaseAddress = new Uri("http://localhost:5005/api/v1/");
            });

            services.AddScoped(iServiceProvider => iServiceProvider.GetService<IHttpClientFactory>().CreateClient("OrderAPI"));
        }

        public static void ConfigureIventoryHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IInventoryHttpRepository, InventoryHttpRepository>("InventoryAPI", (_serviceProvider, httpClient) =>
            {
                httpClient.BaseAddress = new Uri("http://localhost:5006/api/");
            });

            services.AddScoped(iServiceProvider => iServiceProvider.GetService<IHttpClientFactory>().CreateClient("InventoryAPI"));
        }

        public static void ConfigureBasketHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IBasketHttpRepository, BasketHttpRepository>("BasketAPI", (_serviceProvider, httpClient) =>
            {
                httpClient.BaseAddress = new Uri("http://localhost:5004/api/");
            });

            services.AddScoped(iServiceProvider => iServiceProvider.GetService<IHttpClientFactory>().CreateClient("BasketAPI"));
        }
    }
}
