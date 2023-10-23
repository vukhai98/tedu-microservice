using Shared.Configurations;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;
using Ocelot.Cache.CacheManager;

namespace OcelotApiGw.Extensions
{
    public static class ServiceExtensions
    {
        //public static IServiceCollection ConfigureServices(this IServiceCollection services)
        //{

        //    return services;
        //}

        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }

        public static void ConfigureOcelot(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOcelot(configuration).AddPolly().AddCacheManager(x =>
            {
                x.WithDictionaryHandle();
            });

            services.AddSwaggerForOcelot(configuration, x =>
            {
                x.GenerateDocsForGatewayItSelf = false;
            });
        }

        public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            var origins = configuration["AllowOrigins"];

            services.AddCors(x =>
            {
                x.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins(origins)
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            return services;
        }

    }
}
