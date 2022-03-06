using AutoMapper;
using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using EventBus.Messages.Events;
using MassTransit;
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
        private readonly DsicountGrpcService dsicountGrpcService;
        private readonly IMapper mapper;
        private readonly IPublishEndpoint  publishEndpoint;

        public BasketController(IBasketRepostiory basketRepository, DsicountGrpcService dsicountGrpcService, IMapper mapper, 
            IPublishEndpoint publishEndpoint)
        {
            this.basketRepository = basketRepository;
            this.dsicountGrpcService = dsicountGrpcService;
            this.mapper = mapper;
            this.publishEndpoint = publishEndpoint;
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
        public async Task<ActionResult<ShoppingCart>> UpdateBasket(ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon= await dsicountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }
            return (Ok(await basketRepository.UpdateBasket(basket)));
        }

        [HttpDelete("{userName}")]
        public async Task<ActionResult<ShoppingCart>> DeleteBasket(string userName)
        {
            await basketRepository.DeleteBasket(userName);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Checkout(BasketCheckout basketCheckout)
        {
            var basket=await basketRepository.GetBasket(basketCheckout.UserName);
            if(basket==null)
                return BadRequest();

            //publish event to rbbitMq
            var evevntMsg = mapper.Map<BasketCheckoutEvent>(basketCheckout);
            evevntMsg.TotalPrice = (int)basket.TotalPrice;
            await publishEndpoint.Publish(evevntMsg);

            await basketRepository.DeleteBasket(basket.UserName);
            return Accepted();
        }
    }
}
