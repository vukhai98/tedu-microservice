using Basket.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Saga.Orchestrator.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Saga.Orchestrator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("{userName}")]
        public async Task<IActionResult> Checkout([Required] string userName, [FromBody] BasketCheckoutDto basketCheckoutDto)
        {
            var result = await _checkoutService.CheckoutOrder(userName, basketCheckoutDto);

            return Ok(result);
        }
    }
}
