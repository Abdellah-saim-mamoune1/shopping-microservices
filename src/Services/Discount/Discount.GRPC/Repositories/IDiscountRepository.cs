using Discount.GRPC.Entities;

namespace Discount.GRPC.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon?> GetDiscount(string BookId);
    }
}
