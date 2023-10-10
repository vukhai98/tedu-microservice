﻿using Shared.Configurations;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ocelot.DependencyInjection;

namespace OcelotApiGw.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {

            return services;
        }

        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }

        public static void ConfigureOcelot(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOcelot(configuration);
        }

        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
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
            services.AddOcelot(configuration);
        }

    }
}
