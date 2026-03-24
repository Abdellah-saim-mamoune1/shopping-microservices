namespace Discount.API.Dtos
{
    public class CouponDto
    {
        public string BookId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; }
    }
}
