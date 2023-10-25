using Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders.Commands.Create;
using Ordering.Application.Features.V1.Orders.Commands.Delete;
using Ordering.Application.Features.V1.Orders.Commands.Update;
using Ordering.Application.Features.V1.Orders.Queries.GetOrders;
using Shared.SeedWork;
using Shared.Services.Email;
using System.Net;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISmtpEmailService _smtpEmailService;

        public OrderController(IMediator mediator, ISmtpEmailService smtpEmailService)
        {
            _mediator = mediator;
            _smtpEmailService = smtpEmailService;
        }
        private static class RouteNames
        {
            public const string GetOrders = nameof(GetOrders);
            public const string CreateOrders = nameof(CreateOrders);
            public const string UpdateOrders = nameof(UpdateOrders);
            public const string DeleteOrders = nameof(DeleteOrders);
        }

        [HttpGet(Name = RouteNames.GetOrders)]
        [ProducesResponseType(typeof(ApiResult<List<OrderDto>>), (int)HttpStatusCode.OK)]
        public async Task<ApiResult<List<OrderDto>>> GetOrderByUserName(string userName)
        {
            var query = new GetOrdersQuery(userName);
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpPost(Name = RouteNames.CreateOrders)]
        [ProducesResponseType(typeof(ApiResult<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiResult<long>> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPut("{id}", Name = RouteNames.UpdateOrders)]
        [ProducesResponseType(typeof(ApiResult<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ApiResult<OrderDto>> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }


        [HttpDelete("{id}", Name = RouteNames.DeleteOrders)]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<long> DeleteOrder([FromBody] DeleteOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpGet("SendMail")]
        public async Task<IActionResult> TestEmail()
        {
            var message = new MailRequest
            {
                Body = "Hello !",
                Subject = "Test",
                ToAddress = "khaivm@runsystem.net"
            };

             _smtpEmailService.SendEmail(message);
            return Ok();
        }
    }
}
