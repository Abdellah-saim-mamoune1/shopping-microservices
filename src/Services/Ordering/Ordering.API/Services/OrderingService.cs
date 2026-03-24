using Ordering.API.Dtos;
using Ordering.API.Repositories;
using Ordering.API.Validators;

namespace Ordering.API.Services
{
    public class OrderingService : IOrderingService
    {
        private readonly IOrderingRepository _orderingRepository;

        public OrderingService(IOrderingRepository orderingRepository)
        {
            _orderingRepository = orderingRepository;
        }

       
        public async Task<ApiResponseDto<object>> CheckOutOrderAsync(OrderCheckoutDto orderDto, int userId)
        {
            var validator = new OrderCheckoutDtoValidator();
            var result = await validator.ValidateAsync(orderDto);

            if (!result.IsValid)
            {
                var errors = result.Errors
                    .Select(e => new ValidationErorrsDto
                    {
                        FieldId = e.PropertyName,
                        Message = e.ErrorMessage
                    }).ToList();

                return UApiResponderDto<object>.BadRequest(errors);
            }

          
            if (orderDto.Items == null || !orderDto.Items.Any())
            {
                return UApiResponderDto<object>.BadRequest(new List<ValidationErorrsDto>
                {
                    new ValidationErorrsDto
                    {
                        FieldId = "Items",
                        Message = "Order must contain at least one item."
                    }
                });
            }

            await _orderingRepository.CheckOutOrder(orderDto, userId);

            return UApiResponderDto<object>.Ok(null, "Order was created successfully.");
        }

        
        public async Task<ApiResponseDto<object>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderingRepository.GetOrdersByUserIdAsync(userId);

            return UApiResponderDto<object>.Ok(orders, "Orders fetched successfully.");
        }

      
        public async Task<ApiResponseDto<object>> GetOrdersPaginatedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                var errors = new List<ValidationErorrsDto>
                {
                    new ValidationErorrsDto
                    {
                        FieldId = "PageNumber",
                        Message = "Page Number must be >= 1."
                    },
                    new ValidationErorrsDto
                    {
                        FieldId = "PageSize",
                        Message = "Page Size must be >= 1."
                    }
                };

                return UApiResponderDto<object>.BadRequest(errors);
            }

            var orders = await _orderingRepository.GetOrdersPaginatedAsync(pageNumber, pageSize);

            return UApiResponderDto<object>.Ok(orders, "Orders fetched successfully.");
        }

     
        public async Task<ApiResponseDto<object>> GetStatsAsync()
        {
            var stats = await _orderingRepository.GetStatsAsync();

            return UApiResponderDto<object>.Ok(stats, "Statistics fetched successfully.");
        }



        public async Task<ApiResponseDto<object>> UpdateOrderStatusAsync(string OrderId, string status)
        {
           
            await _orderingRepository.UpdateOrderStatusAsync(OrderId,status);

            return UApiResponderDto<object>.Ok( "Order updated successfully.");
        }


    }
}