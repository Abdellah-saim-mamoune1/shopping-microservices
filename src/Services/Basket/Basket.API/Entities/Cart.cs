namespace Basket.API.Entities
{
    public class Cart
    {
        public int UserId { get; set; } 

        public List<CartItem> Items { get; set; } = new();

        public decimal TotalPrice
        {
            get
            {
                decimal totalprice = 0;
                foreach (var item in Items)
                {
                    totalprice += item.Price * item.Quantity;
                }
                return totalprice;
            }
        }
    }

}
