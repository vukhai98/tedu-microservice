using AutoMapper;
using EventBus.Messages.IntegrationEvents.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.V1.Orders.Commands.Create;
using ILogger = Serilog.ILogger;

namespace Ordering.API.Application.IntegrationEvents.EventsHandler
{
    public class BasketCheckoutEventHandler : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketCheckoutEventHandler> _logger;
        public BasketCheckoutEventHandler(ILogger<BasketCheckoutEventHandler> logger, IMapper mapper, IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = _mapper.Map<CreateOrderCommand>(context.Message);

            // Gửi xuống handler để tạo  đơn hàng 
            var result = await _mediator.Send(command);
            _logger.LogInformation("BasketCheckoutEvent consumed successfully. Order is created with Id:{newOrderId}", result.Data);
        }
    }
}
