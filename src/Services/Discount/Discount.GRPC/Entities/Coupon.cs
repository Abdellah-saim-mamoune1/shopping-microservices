using System.ComponentModel.DataAnnotations;

namespace Discount.GRPC.Entities
{
    public class Coupon
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string BookId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; }

    }
}
