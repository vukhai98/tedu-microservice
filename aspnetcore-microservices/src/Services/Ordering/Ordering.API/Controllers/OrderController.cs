using Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Models;
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

        [HttpGet("GetOrderByUserName")]
        public async Task<ApiResult<List<OrderDto>>> GetOrderByUserName(string userName)
        {
            var query = new GetOrdersQuery(userName);
            var result = await _mediator.Send(query);
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

            await _smtpEmailService.SendEmailAsync(message);
            return Ok();
        }
    }
}
