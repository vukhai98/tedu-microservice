using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Commands.Delete
{
    public class DeleteOrderCommand : IRequest<long>
    {
        public long Id { get;  set; }

    }
}
