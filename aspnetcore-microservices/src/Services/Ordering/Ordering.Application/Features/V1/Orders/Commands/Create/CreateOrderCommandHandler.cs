using AutoMapper;
using Contracts.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Common.Intrerfaces;
using Ordering.Application.Features.V1.Orders.Commands.Create;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using Shared.Services.Email;

namespace Ordering.Application.Features.V1.Orders.Commands.Create
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOrderHandler> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly ISmtpEmailService _emailService;
        private const string MethodName = "CreateOrderCommandHandler";
        public CreateOrderHandler(IMapper mapper, IOrderRepository orderRepository, ILogger<CreateOrderHandler> logger, ISmtpEmailService emailService)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _orderRepository = orderRepository ?? throw new ArgumentException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
        }

        public async Task<ApiResult<long>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"BEGIN: {MethodName} - UserName: {command.UserName}");

                var orderEntity = _mapper.Map<Order>(command);

                var addedOrder = await _orderRepository.CreateOrderAsync(orderEntity);

                await _orderRepository.SaveChangesAsync();

                _logger.LogInformation($"Order {addedOrder.Id} is successfully created.");

                // send mail for user 
                SendMailAsync(addedOrder, cancellationToken);

                return new ApiSuccessedResult<long>(addedOrder.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new ApiResult<long>(false, 0);
            }
        }

        private async Task SendMailAsync(Order order, CancellationToken cancellationToken)
        {
            var emailRequest = new MailRequest
            {
                ToAddress = order.EmailAddress,
                Body = "Order was created.",
                Subject = "Order was created"
            };

            try
            {
                await _emailService.SendEmailAsync(emailRequest, cancellationToken);
                _logger.LogInformation($"Sent create Orders to mail {order.EmailAddress}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Orders {order.Id} failed due to an erroe with the mail service: {ex.Message}");
            }
        }
    }
}
