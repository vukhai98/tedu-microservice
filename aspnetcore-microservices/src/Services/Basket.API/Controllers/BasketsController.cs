using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories.Interfaces;
using Basket.API.Services;
using Basket.API.Services.Interfaces;
using EventBus.Messages.IntegrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Shared.DTOs.Baskets;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly StockItemGrpcService _stockItemGrpcService;

        private readonly IMapper _mapper;


        private readonly IPublishEndpoint _publishEndpoint;

        public BasketsController(IBasketRepository repository, IMapper mapper, IPublishEndpoint publishEndpoint, StockItemGrpcService stockItemGrpcService)
        {
            _repository = repository;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _stockItemGrpcService = stockItemGrpcService;

        }
        [HttpGet("{userName}")]
        public async Task<ActionResult<CartDTO>> GetBasketByUserName([Required] string userName)
        {
            var cart = await _repository.GetBasketByUserName(userName);

            var result = _mapper.Map<CartDTO>(cart);

            return Ok(result);
        }

        [HttpPut("updateBasket")]
        public async Task<IActionResult> UpdateBasket([FromBody] CartDTO request)
        {
            // Consumer Grpc Services 
            foreach (var item in request.Items)
            {
                var quantityRespone = await _stockItemGrpcService.GetStock(item.ItemNo);

                item.SetAvaliableItemPrice(quantityRespone.Quantity);
            }
            var options = new DistributedCacheEntryOptions()
                                        .SetAbsoluteExpiration(DateTime.UtcNow.AddHours(1)) // Hạn của basket tồn tại trong bao lâu
                                        .SetSlidingExpiration(TimeSpan.FromHours(1)); // Kiểm tra  xem bao nhiêu lâu chưa gọi đến key đấy

            var cart = _mapper.Map<Cart>(request);
            var updateCart = await _repository.UpdateBasket(cart, options);
            var result = _mapper.Map<CartDTO>(updateCart);

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
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;

            _publishEndpoint.Publish(eventMessage);

            // remove the basket 
            await _repository.DeleteBasketFromUserName(basketCheckout.UserName);

            return Accepted();
        }
    };
}