using Basket.API.Dtos;
using Basket.API.Entities;

namespace Basket.API.Services
{
    public interface IBasketService
    {
        Task<ApiResponseDto<object>> GetAsync(string userId);
        Task<ApiResponseDto<object>> AddItemAsync(CartItem item, string UserId);
        Task<ApiResponseDto<object>> UpdateItemAsync(BasketUpdateDto item,string UserId);
        Task<ApiResponseDto<object>> DeleteItemAsync(string itemId,string UserId);
        Task<ApiResponseDto<object>> CheckoutItemAsync(BasketCheckoutDto basketCheckout, int UserId);

    }
}
