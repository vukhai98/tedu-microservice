using MediatR;
using Shared.DTOs.Orders;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<ApiResult<OrderDto>>
    {
        public long Id { get; set; }
    }
}
