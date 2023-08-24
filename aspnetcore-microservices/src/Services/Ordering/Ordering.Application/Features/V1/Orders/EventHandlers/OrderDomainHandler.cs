using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.OrderAggregate.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.EventHandlers
{
    public class OrderDomainHandler : INotificationHandler<OrderCreatedEvent>, INotificationHandler<OrderDeletedEvent>
    {
        private readonly ILogger<OrderDomainHandler> _logger;

        public OrderDomainHandler(ILogger<OrderDomainHandler> logger)
        {
            _logger = logger;
        }
        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Handle(OrderDeletedEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
