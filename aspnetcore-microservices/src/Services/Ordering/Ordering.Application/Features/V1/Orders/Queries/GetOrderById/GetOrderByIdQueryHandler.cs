using AutoMapper;
using MediatR;
using Ordering.Application.Common.Intrerfaces;
using Shared.DTOs.Orders;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ApiResult<OrderDto>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdQueryHandler(IMapper mapper, IOrderRepository orderRepository)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _orderRepository = orderRepository ?? throw new ArgumentException(nameof(orderRepository));
        }

        public async Task<ApiResult<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var orderEntities = await _orderRepository.GetByIdAsync(request.Id);

            var orders = _mapper.Map<OrderDto>(orderEntities);

            var result = new ApiSuccessedResult<OrderDto>(orders);

            return result;
        }
    }
}
