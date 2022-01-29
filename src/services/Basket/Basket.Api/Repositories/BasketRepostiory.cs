using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public class BasketRepostiory : IBasketRepostiory
    {
        private readonly IDistributedCache redisCache;

        public BasketRepostiory(IDistributedCache redisCache)
        {
            this.redisCache = redisCache;
        }
        public async Task DeleteBasket(string userName)
        {
           await redisCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
           var basket=await redisCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await redisCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));
            return await GetBasket(basket.UserName);
        }
    }
}
