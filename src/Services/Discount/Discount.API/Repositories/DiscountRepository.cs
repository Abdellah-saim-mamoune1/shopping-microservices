using Discount.API;
using Discount.API.Dtos;
using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.EntityFrameworkCore;

public class DiscountRepository : IDiscountRepository
{
    private readonly AppDbContext _dbContext;

    public DiscountRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<string> CreateDiscount(CouponDto coupon)
    {
        var c = new Coupons
        {
            BookId = coupon.BookId,
            Amount = coupon.Amount,
            Name = coupon.Name
        };

        _dbContext.Coupons.Add(c);

        await _dbContext.SaveChangesAsync();

        return c.Id.ToString();

    }

    public async Task<bool> ExistsDiscount(string bookId)
    {
        return await _dbContext.Coupons.AnyAsync(c => c.BookId == bookId);
    }

    public async Task<Coupons?> GetDiscountById(string Id)
    {
        
        var c =await _dbContext.Coupons.FirstOrDefaultAsync(c=>c.Id.ToString()==Id);

        return c;

    }


    public async Task<List<Coupons>> GetAll()
    {

        return await _dbContext.Coupons.ToListAsync();

    }


    public async Task UpdateDiscount(string Id, CouponDto coupon)
    {

        var discount = await _dbContext.Coupons.Where(c => c.Id == Guid.Parse(Id)).FirstAsync();

        discount.Amount=coupon.Amount;
       
        await _dbContext.SaveChangesAsync();

    }

    public async Task DeleteDiscount(string Id)
    {
        var coupon = await _dbContext.Coupons
            .FirstAsync(c => c.Id == Guid.Parse(Id));


        _dbContext.Coupons.Remove(coupon);
        await _dbContext.SaveChangesAsync();
    }
}
