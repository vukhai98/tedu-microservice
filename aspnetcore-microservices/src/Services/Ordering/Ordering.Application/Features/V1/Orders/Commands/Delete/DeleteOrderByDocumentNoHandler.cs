using AutoMapper;
using Contracts.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Intrerfaces;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using ILogger = Serilog.ILogger;


namespace Ordering.Application.Features.V1.Orders.Commands.Delete
{
    public class DeleteOrderByDocumentNoHandler : IRequestHandler<DeleteOrderByDocumentNoCommand, ApiResult<bool>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly ISmtpEmailService _emailService;
        private const string MethodName = "UpdateOrderCommandHandler";
        public DeleteOrderByDocumentNoHandler(IMapper mapper, IOrderRepository orderRepository, ILogger logger, ISmtpEmailService emailService)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _orderRepository = orderRepository ?? throw new ArgumentException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
        }

        public async Task<ApiResult<bool>> Handle(DeleteOrderByDocumentNoCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var orderOld = await _orderRepository.GetOrderByDocumentNo(command.DocumentNo);

                if (orderOld == null)
                    throw new NotFoundException(nameof(Order), command.DocumentNo);

                 _orderRepository.Delete(orderOld);
                orderOld.DeletedOrder();
                await _orderRepository.SaveChangesAsync();

                _logger.Information($"Order {orderOld.Id} is successfully deleted.");

                return new ApiResult<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return new ApiResult<bool>(false); 
            }
        }
    }
}
