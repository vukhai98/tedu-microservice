using Contracts.Common.Events;
using Infrastructure.Common;
using MediatR;
using Microsoft.Extensions.Logging;

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
