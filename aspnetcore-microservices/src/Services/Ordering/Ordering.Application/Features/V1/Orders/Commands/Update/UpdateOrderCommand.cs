using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Application.Features.V1.Orders.Common;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Commands.Update
{
    public class UpdateOrderCommand : CreateOrUpdateCommand, IRequest<ApiResult<Shared.DTOs.Orders.OrderDto>>
    {
        public long Id { get; set; }

        public void SetId(long id)
        {
            Id = id;
        }

        
    }
}
