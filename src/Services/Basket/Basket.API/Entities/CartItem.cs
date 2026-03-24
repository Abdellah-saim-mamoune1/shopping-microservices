namespace Basket.API.Entities
{
    public class CartItem
    {
        public int Quantity { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal DiscountAmount { get; set; }

        public decimal CalculateTotalPrice()
        {
            return (Price-DiscountAmount)*Quantity;
        }

    }

}
