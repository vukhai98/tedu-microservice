using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Common.Intrerfaces;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders.Commands.CreateOrder;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Queries.GetOrders
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IMapper mapper, IOrderRepository orderRepository, ILogger logger)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _orderRepository = orderRepository ?? throw new ArgumentException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public async Task<ApiResult<long>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            try
            {
                return new ApiResult<long>(false, 0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new ApiResult<long>(false, 0);
            }
        }
    }
}
