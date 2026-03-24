using Ordering.API.Entities;

namespace Ordering.API.Dtos
{
    public class OrderCheckoutDto
    {
        public List<Item> Items { get; set; } = new();

        public decimal TotalPrice { get; set; }

        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public int PaymentMethod { get; set; }
    }
}
