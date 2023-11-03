using AutoMapper;
using Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.V1.Orders.Commands.Create;
using Ordering.Application.Features.V1.Orders.Commands.Delete;
using Ordering.Application.Features.V1.Orders.Commands.Update;
using Ordering.Application.Features.V1.Orders.Queries.GetOrders;
using Ordering.Application.Features.V1.Orders.Queries.GetOrderById;

using Shared.DTOs.Orders;
using Shared.SeedWork;
using Shared.Services.Email;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ISmtpEmailService _smtpEmailService;

        public OrderController(IMediator mediator, ISmtpEmailService smtpEmailService, IMapper mapper)
        {
            _mediator = mediator;
            _smtpEmailService = smtpEmailService;
            _mapper = mapper;
        }
        private static class RouteNames
        {
            public const string GetOrders = nameof(GetOrders);
            public const string GetOrder = nameof(GetOrder);
            public const string CreateOrders = nameof(CreateOrders);
            public const string UpdateOrders = nameof(UpdateOrders);
            public const string DeleteOrders = nameof(DeleteOrders);
            public const string DeleteOrderByDocumentNo = "document-no";
        }

        [HttpGet(Name = RouteNames.GetOrders)]
        [ProducesResponseType(typeof(ApiResult<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ApiResult<List<OrderDto>>> GetOrderByUserName(string userName)
        {
            var query = new GetOrdersQuery(userName);
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResult<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ApiResult<OrderDto>> GetOrderById(long id)
        {
            var query = new GetOrderByIdQuery
            {
                Id = id
            };

            var result = await _mediator.Send(query);

            return result;
        }

        [HttpPost(Name = RouteNames.CreateOrders)]
        [ProducesResponseType(typeof(ApiResult<long>), (int)HttpStatusCode.OK)]
        public async Task<ApiResult<long>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var command = _mapper.Map<CreateOrderCommand>(createOrderDto);
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


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        public async Task<long> DeleteOrder([Required] long id)
        {
            var command = new DeleteOrderCommand
            {
                Id = id
            };

            var result = await _mediator.Send(command);

            return result;
        }

        [HttpDelete("documentno/{documentNo}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ApiResult<bool>> DeleteOrderByDocumentNO([Required] string documentNo)
        {
            var command = new DeleteOrderByDocumentNoCommand
            {
                DocumentNo = documentNo
            };
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
