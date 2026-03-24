using System.ComponentModel.DataAnnotations;

namespace Discount.API.Entities
{
    public class Coupons
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();  
        public string BookId { get; set; }=string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; }

    }
}
