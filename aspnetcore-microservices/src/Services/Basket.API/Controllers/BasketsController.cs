using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _repository;

        private readonly IMapper _mapper;

        private readonly IPublishEndpoint _publishEndpoint;

        public BasketsController(IBasketRepository repository, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }
        [HttpGet("{userName}")]
        public async Task<IActionResult> GetBasketByUserName([Required] string userName)
        {
            var result = await _repository.GetBasketByUserName(userName);

            return Ok(result ?? new Cart());
        }

        [HttpPut("updateBasket")]
        public async Task<IActionResult> UpdateBasket([FromBody] Cart cart)
        {
            var options = new DistributedCacheEntryOptions()
                                        .SetAbsoluteExpiration(DateTime.UtcNow.AddHours(1)) // Hạn của basket tồn tại trong bao lâu
                                        .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Kiểm tra  xem bao nhiêu lâu chưa gọi đến key đấy
            var result = await _repository.UpdateBasket(cart, options);

            return Ok(result);
        }

        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteBasket([Required] string userName)
        {
            var result = await _repository.DeleteBasketFromUserName(userName);

            return Ok(result);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _repository.GetBasketByUserName(basketCheckout.UserName);

            if (basket == null)
                return NotFound();

            // publish checkout event to Eventbus Message
            var eventMessage = _mapper.Map<BasketCheckout>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;

             _publishEndpoint.Publish(eventMessage);

            // remove the basket 
            await _repository.DeleteBasketFromUserName(basketCheckout.UserName);

            return Accepted();
        }

    };
}