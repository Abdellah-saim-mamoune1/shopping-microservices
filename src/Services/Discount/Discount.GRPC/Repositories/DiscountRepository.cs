using Discount.GRPC;
using Discount.GRPC.Entities;
using Discount.GRPC.Repositories;
using Microsoft.EntityFrameworkCore;

public class DiscountRepository : IDiscountRepository
{
    private readonly AppDbContext _dbContext;

    public DiscountRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Coupon?> GetDiscount(string BookId)
    {
        var coupon = await _dbContext.Coupons
            .FirstOrDefaultAsync(c => c.BookId == BookId);

        return coupon == null ? null : coupon;
         
    }

   
}
