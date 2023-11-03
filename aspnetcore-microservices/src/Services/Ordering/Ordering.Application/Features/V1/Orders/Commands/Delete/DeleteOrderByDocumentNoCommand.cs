using MediatR;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Commands.Delete
{
    public class DeleteOrderByDocumentNoCommand : IRequest<ApiResult<bool>>
    {
        public string DocumentNo { get;  set; }

    }
}
