using AutoMapper;
using Contracts.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Core;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Intrerfaces;
using Ordering.Application.Features.V1.Orders.Commands.Create;
using Ordering.Domain.Entities;
using Shared.DTOs.Orders;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;


namespace Ordering.Application.Features.V1.Orders.Commands.Update
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, ApiResult<OrderDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly ISmtpEmailService _emailService;
        private const string MethodName = "UpdateOrderCommandHandler";
        public UpdateOrderHandler(IMapper mapper, IOrderRepository orderRepository, ILogger logger, ISmtpEmailService emailService)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _orderRepository = orderRepository ?? throw new ArgumentException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
        }

        public async Task<ApiResult<OrderDto>> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var orderOld = await _orderRepository.GetByIdAsync(command.Id);

                if (orderOld == null)
                    throw new NotFoundException(nameof(Order), command.Id);

                _logger.Information($"BEGIN: {MethodName} - Order: {command.Id}");

                var orderEntity = _mapper.Map<Order>(command);

                var updatedOrder = await _orderRepository.UpdateOrderAsync(orderEntity);

                await _orderRepository.SaveChangesAsync();

                _logger.Information($"Order {updatedOrder.Id} is successfully created.");

                var result = _mapper.Map<OrderDto>(updatedOrder);

                _logger.Information($"END {MethodName} - Order: {command.Id}");

                return new ApiSuccessedResult<OrderDto>(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return new ApiErrorResult<OrderDto>("Update order failed.");
            }
        }
    }
}
