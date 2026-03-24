using Discount.API.Dtos;
using Discount.API.Entities;

namespace Discount.API.Repositories
{
    public interface IDiscountRepository
    {
        public Task<string> CreateDiscount(CouponDto coupon);
        public Task UpdateDiscount(string Id, CouponDto coupon);
        public Task DeleteDiscount(string Id);
        public Task<Coupons?> GetDiscountById(string Id);
        public Task<bool> ExistsDiscount(string bookId);
        public Task<List<Coupons>> GetAll();
    }
}
