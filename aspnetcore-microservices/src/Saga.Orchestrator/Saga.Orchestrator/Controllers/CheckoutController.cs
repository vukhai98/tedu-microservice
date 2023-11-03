using Basket.API.Entities;
using Contracts.Sages.SagaOrderManager;
using Microsoft.AspNetCore.Mvc;
using Saga.Orchestrator.OrderManager;
using Saga.Orchestrator.Services.Interfaces;
using Shared.DTOs.Inventory;
using System.ComponentModel.DataAnnotations;

namespace Saga.Orchestrator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        private readonly ISagaOrderManager<BasketCheckoutDto, OrderRespone> _sagaOrderManager;
        public CheckoutController(ICheckoutService checkoutService, ISagaOrderManager<BasketCheckoutDto, OrderRespone> sagaOrderManager)
        {
            _checkoutService = checkoutService;
            _sagaOrderManager = sagaOrderManager;
        }

        [HttpPost("{userName}")]
        public async Task<IActionResult> Checkout([Required] string userName, [FromBody] BasketCheckoutDto basketCheckoutDto)
        {
            var result = await _checkoutService.CheckoutOrder(userName, basketCheckoutDto);

            return Ok(result);
        }


        [HttpPost("saleorder/{userName}")]
        public OrderRespone CheckoutSaleOrder([Required] string userName, [FromBody] BasketCheckoutDto basketCheckoutDto)
        {
            basketCheckoutDto.UserName = userName;

            var result = _sagaOrderManager.CreateOrder(basketCheckoutDto);

            return result;
        }
    }
}
