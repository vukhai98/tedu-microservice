using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Customer.API.Persistence;
using System.Configuration;
using Customer.API;
using Microsoft.AspNetCore.Builder;
using Customer.API.Services.Interfaces;
using Customer.API.Services;

namespace Customer.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddEndpointsApiExplorer();
            services.AddInfrastructureServices();
            services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));

            return services;

        }
       
        private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            return services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>))
                           .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                           .AddScoped(typeof(ICustomerService), typeof(CustomerService));
        }
    }
}
