using Contracts.Common.Events;
using Contracts.Common.Interfaces;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class MediatorExtensions
    {
        public static async Task DispatchDomainEventAsync(this IMediator mediator, List<BaseEvent> domainEvents, ILogger logger)
        {

            foreach (var item in domainEvents)
            {
                await mediator.Publish(item);

                var data = new SerializeService().Serialize(item);

                logger.LogInformation($"\n-----\n A domain event has been published!\n" +
                                      $"Event: {item.GetType().Name}\n" +
                                      $"Data: {data}\n-----\n");
            }

        }
    }
}
