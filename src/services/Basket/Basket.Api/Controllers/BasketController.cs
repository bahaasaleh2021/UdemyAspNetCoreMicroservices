using Basket.Api.Entities;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepostiory basketRepository;

        public BasketController(IBasketRepostiory basketRepository)
        {
            this.basketRepository = basketRepository;
        }


        [HttpGet("{userName}")]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            try
            {
                return Ok(await basketRepository.GetBasket(userName) ?? new ShoppingCart());

            }
            catch (RedisConnectionException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (RedisException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> AddBasket(ShoppingCart basket)
        {
            return (Ok(await basketRepository.UpdateBasket(basket)));
        }

        [HttpDelete("{userName}")]
        public async Task<ActionResult<ShoppingCart>> DeleteBasket(string userName)
        {
            await basketRepository.DeleteBasket(userName);
            return Ok();
        }
    }
}
