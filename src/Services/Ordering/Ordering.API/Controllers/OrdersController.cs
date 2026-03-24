using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Dtos;
using Ordering.API.Services;
using System.Security.Claims;

namespace Ordering.API.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderingService _orderingService;

        public OrdersController(IOrderingService orderingService)
        {
            _orderingService = orderingService;
        }

     
        [Authorize(Roles = "Admin")]
        [HttpGet("all/{pageNumber:int},{pageSize:int}")]
        public async Task<ActionResult> GetAllOrdersPaginated(int pageNumber, int pageSize)
        {
            var data = await _orderingService.GetOrdersPaginatedAsync(pageNumber, pageSize);
            return StatusCode(data.StatusCode, data);
        }

      
        [Authorize(Roles = "Admin")]
        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult> GetOrdersByUserIdByAdmin(int userId)
        {
            var data = await _orderingService.GetOrdersByUserIdAsync(userId);
            return StatusCode(data.StatusCode, data);
        }

     
        [Authorize(Roles = "Client")]
        [HttpGet]
        public async Task<ActionResult> GetMyOrders()
        {
            var data = await _orderingService.GetOrdersByUserIdAsync(GetUserId());
            return StatusCode(data.StatusCode, data);
        }

     
        [Authorize(Roles = "Client")]
        [HttpPost]
        public async Task<ActionResult> CheckOutOrder([FromBody] OrderCheckoutDto order)
        {
            var data = await _orderingService.CheckOutOrderAsync(order, GetUserId());
            return StatusCode(data.StatusCode, data);
        }

     
        [Authorize(Roles = "Admin")]
        [HttpGet("stats")]
        public async Task<ActionResult> GetStats()
        {
            var data = await _orderingService.GetStatsAsync();
            return StatusCode(data.StatusCode, data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{OrderId},{Status}")]
        public async Task<ActionResult> GetStats(string OrderId, string Status)
        {
            var data = await _orderingService.UpdateOrderStatusAsync(OrderId, Status);
            return StatusCode(data.StatusCode, data);
        }
        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            return int.Parse(claim.Value);
        }
    }
}