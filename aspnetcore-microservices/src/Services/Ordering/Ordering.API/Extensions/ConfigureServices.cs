using Contracts.Common.Interfaces;
using Contracts.Services;
using FluentValidation;
using Infrastructure.Common;
using Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Common.Behaviours;
using Ordering.Application.Common.Intrerfaces;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders.Commands.Create;
using Ordering.Application.Features.V1.Orders.Commands.Delete;
using Ordering.Application.Features.V1.Orders.Commands.Update;
using Ordering.Application.Features.V1.Orders.Queries.GetOrders;
using Ordering.Infrastructure.Repositories;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.API.Extensions
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationBehavour<,>));


            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped(typeof(ISmtpEmailService), typeof(SMTPEmailServices));

            // Đăng ký xử lý yêu cầu GetOrdersQuery
            services.AddTransient<IRequestHandler<GetOrdersQuery, ApiResult<List<OrderDto>>>, GetOrdersQueryHandler>();
            services.AddTransient<IRequestHandler<CreateOrderCommand, ApiResult<long>>, CreateOrderHandler>();
            services.AddTransient<IRequestHandler<UpdateOrderCommand, ApiResult<OrderDto>>, UpdateOrderHandler>();
            services.AddTransient<IRequestHandler<DeleteOrderCommand, long>, DeleteOrderHandler>();

            return services;
        }
    }
}
