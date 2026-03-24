using Ordering.API.Entities;

namespace Ordering.API.Dtos
{
    public class OrdersPaginatedGet
    {
        public List<Order> Orders { get; set; } = new();
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
