﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Common.Behaviours;
using Ordering.Application.Common.Intrerfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
            services.AddAutoMapper(Assembly.GetExecutingAssembly())
                    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                    .AddMediatR(Assembly.GetExecutingAssembly())
                    .AddTransient(typeof(IPipelineBehavior<,>),typeof(UnhandledExceptionBehaviour<,>))
                    .AddTransient(typeof(IPipelineBehavior<,>),typeof(PerformanceBehaviour<,>))
                    .AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavour<,>))
                    //.AddScoped(IOrderRepository, OrderRepository)
                    ;
    }
}
