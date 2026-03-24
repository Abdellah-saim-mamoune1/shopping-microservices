using Basket.API.Dtos;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task<Cart?> GetAsync(string UserId)
        {
            var basket = await _redisCache.GetStringAsync(UserId);

            if (String.IsNullOrEmpty(basket))
                return null;

            return JsonConvert.DeserializeObject<Cart>(basket);
        }

        public async Task AddItemAsync(CartItem item, string userId)
        {
            var basketString = await _redisCache.GetStringAsync(userId);

            Cart basket;

            if (string.IsNullOrEmpty(basketString))
            {
                basket = new Cart
                {
                    UserId = int.Parse(userId),
                    Items = new List<CartItem>()
                };
            }
            else
            {
                basket = JsonConvert.DeserializeObject<Cart>(basketString);
            }


            var Item = basket.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (Item != null)
            {
                Item.Quantity++;
            }

            else
                basket.Items.Add(item);

            var updatedBasket = JsonConvert.SerializeObject(basket);

            await _redisCache.SetStringAsync(userId, updatedBasket);
        }

        public async Task UpdateItemAsync(BasketUpdateDto Item, string UserId)
        {
            var basketString = await _redisCache.GetStringAsync(UserId);

            if (String.IsNullOrEmpty(basketString))
                return;

            var basket = JsonConvert.DeserializeObject<Cart>(basketString);

            if (basket == null)
                return;

            var item = basket.Items.FirstOrDefault(i => i.ProductId == Item.ProductId);

            if (item == null)
                return;

            item.Quantity = Item.Quantity;

       
            await _redisCache.SetStringAsync(UserId, JsonConvert.SerializeObject(basket));
        }

        public async Task DeleteItemAsync(string ItemId, string UserId)
        {
            var basketString = await _redisCache.GetStringAsync(UserId);

            if (String.IsNullOrEmpty(basketString))
                return;

            var basket = JsonConvert.DeserializeObject<Cart>(basketString);

            if (basket == null)
                return;

            var item = basket.Items.FirstOrDefault(i => i.ProductId == ItemId);

            if (item == null)
                return;

            basket.Items.Remove(item);

            await _redisCache.SetStringAsync(UserId, JsonConvert.SerializeObject(basket));
        }

        public async Task DeleteBasketAsync(string userId)
        {
            await _redisCache.RemoveAsync(userId);
        }
    }
}
