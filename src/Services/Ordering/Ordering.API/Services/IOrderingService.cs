using Ordering.API.Dtos;

namespace Ordering.API.Services
{
    public interface IOrderingService
    {
        public  Task<ApiResponseDto<object>> CheckOutOrderAsync(OrderCheckoutDto orderDto, int UserId);
        public  Task<ApiResponseDto<object>> GetOrdersByUserIdAsync(int userId);
        public  Task<ApiResponseDto<object>> GetOrdersPaginatedAsync(int PageNumber, int PageSize);
        public  Task<ApiResponseDto<object>> GetStatsAsync();
        public  Task<ApiResponseDto<object>> UpdateOrderStatusAsync(string OrderId, string status);
     
    }
}
