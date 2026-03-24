using Discount.API.Dtos;

namespace Discount.API.Services
{
    public interface IDiscountService
    {
        public Task<ApiResponseDto<object>> GetByIdAsync(string Id);
        public Task<ApiResponseDto<object>> GetAllAsync();
        public Task<ApiResponseDto<object>> CreateAsync(CouponDto coupon);
        public Task<ApiResponseDto<object>> UpdateAsync(string id, CouponDto coupon);
        public Task<ApiResponseDto<object>> DeleteAsync(string id);
    }
}
