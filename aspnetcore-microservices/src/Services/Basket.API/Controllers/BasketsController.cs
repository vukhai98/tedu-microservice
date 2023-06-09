using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        public BasketsController(IBasketRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("{userName}")]
        public async Task<IActionResult> GetBasketByUserName([Required]string userName)
        {
            var result = await _repository.GetBasketByUserName(userName);

            return Ok(result ?? new Cart());
        }

        [HttpPost("updateBasket")]
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

    };
}