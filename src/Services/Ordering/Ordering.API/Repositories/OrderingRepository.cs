using MongoDB.Driver;
using Ordering.API.Context;
using Ordering.API.Dtos;
using Ordering.API.Entities;

namespace Ordering.API.Repositories
{
    public class OrderingRepository : IOrderingRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderingRepository(MongoDbContext context)
        {
            _orders = context.Orders
                ?? throw new ArgumentNullException(nameof(context));
        }

      
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _orders
                .Find(o => o.UserId == userId)
                .SortByDescending(o => o.CreatedAt)
                .ToListAsync();
        }


        public async Task UpdateOrderStatusAsync(string OrderId, string status)
        {
            var update = Builders<Order>.Update.Set(o => o.Status, status);

            await _orders.UpdateOneAsync(
                o => o.Id == OrderId,
                update
            );
        }


        public async Task<OrdersPaginatedGet> GetOrdersPaginatedAsync(int pageNumber, int pageSize)
        {
            var totalOrders = await _orders.CountDocumentsAsync(_ => true);

            var orders = await _orders
                .Find(_ => true)
                .SortByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalOrders / (double)pageSize);

            return new OrdersPaginatedGet
            {
                Orders = orders,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }



        public async Task CheckOutOrder(OrderCheckoutDto orderDto, int userId)
        {
            var order = new Order
            {
                UserId = userId,

                Items = orderDto.Items,
                TotalPrice = orderDto.TotalPrice,
                Status = "Processing",
                FirstName = orderDto.FirstName,
                LastName = orderDto.LastName,
                EmailAddress = orderDto.EmailAddress,
                AddressLine = orderDto.AddressLine,
                Country = orderDto.Country,
                State = orderDto.State,
                ZipCode = orderDto.ZipCode,

               
                PaymentMethod = orderDto.PaymentMethod,

                CreatedAt = DateTime.UtcNow
            };

            await _orders.InsertOneAsync(order);
        }

    
        public async Task<OrderStatsDto> GetStatsAsync()
        {
            var totalOrders = await _orders.CountDocumentsAsync(_ => true);

            var revenues = await _orders
                .Find(_ => true)
                .Project(o => o.TotalPrice)
                .ToListAsync();

            return new OrderStatsDto
            {
                TotalOrders = (int)totalOrders,
                TotalRevenue = revenues.Sum()
            };
        }
    }
}