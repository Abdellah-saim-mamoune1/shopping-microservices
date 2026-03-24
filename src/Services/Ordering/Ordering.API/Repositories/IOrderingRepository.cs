using Ordering.API.Dtos;
using Ordering.API.Entities;

namespace Ordering.API.Repositories
{
    public interface IOrderingRepository
    {
        public Task<OrdersPaginatedGet> GetOrdersPaginatedAsync(int pageNumber, int pageSize);
        public Task<List<Order>> GetOrdersByUserIdAsync(int UserId);
        public Task CheckOutOrder(OrderCheckoutDto order, int UserId);
        public Task<OrderStatsDto> GetStatsAsync();
        public Task UpdateOrderStatusAsync(string OrderId, string status);

      
    }
}
