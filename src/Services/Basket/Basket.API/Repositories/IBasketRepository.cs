using Basket.API.Dtos;
using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        public Task<Cart?> GetAsync(string userId);
        public Task AddItemAsync(CartItem Item, string UserId);
        public Task UpdateItemAsync(BasketUpdateDto Item, string UserId);
        public Task DeleteItemAsync(string ItemId, string UserId);
        public Task DeleteBasketAsync(string userId);
    }
}
