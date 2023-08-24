using AutoMapper;
using Contracts.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Core;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Intrerfaces;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders.Commands.Create;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Commands.Delete
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, long>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteOrderHandler> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly ISmtpEmailService _emailService;
        private const string MethodName = "UpdateOrderCommandHandler";
        public DeleteOrderHandler(IMapper mapper, IOrderRepository orderRepository, ILogger<DeleteOrderHandler> logger, ISmtpEmailService emailService)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _orderRepository = orderRepository ?? throw new ArgumentException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentException(nameof(emailService));
        }

        public async Task<long> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var orderOld = await _orderRepository.GetByIdAsync(command.Id);

                if (orderOld == null)
                    throw new NotFoundException(nameof(Order), command.Id);

                 _orderRepository.Delete(orderOld);
                orderOld.DeletedOrder();
                await _orderRepository.SaveChangesAsync();

                _logger.LogInformation($"Order {orderOld.Id} is successfully deleted.");

                return orderOld.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return 0;
            }
        }
    }
}
