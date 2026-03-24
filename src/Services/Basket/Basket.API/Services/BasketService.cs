using Basket.API.Dtos;
using Basket.API.Entities;
using Basket.API.Repositories;
using Basket.API.Validators;
using EventBus.Messages.Events;
using MassTransit;

namespace Basket.API.Services
{
    public class BasketService:IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        public BasketService(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint)
        {
            _basketRepository = basketRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ApiResponseDto<object>> GetAsync(string userId)
        {
           
                if (string.IsNullOrWhiteSpace(userId))
                {
                    var errors = new List<ValidationErorrsDto>
                {
                    new ValidationErorrsDto { FieldId = "UserId", Message = "UserId is required." }
                };

                    return UApiResponderDto<object>.BadRequest(errors);
                }

                var basket = await _basketRepository.GetAsync(userId);


                return UApiResponderDto<object>.Ok(basket, "Basket fetched successfully.");
           
        }

        public async Task<ApiResponseDto<object>> AddItemAsync(CartItem item, string UserId)
        {
           
                List<ValidationErorrsDto> errors = new();
                var validator = new CartItemValidator();
                var result = await validator.ValidateAsync(item);

                if (!result.IsValid)
                {
                    errors = result.Errors
                        .Select(e => new ValidationErorrsDto
                        {
                            FieldId = e.PropertyName,
                            Message = e.ErrorMessage
                        }).ToList();

                    return UApiResponderDto<object>.BadRequest(errors);
                }

                await _basketRepository.AddItemAsync(item,UserId);

                return UApiResponderDto<object>.Ok(null, "Item added to basket successfully.");
           
        }

        public async Task<ApiResponseDto<object>> UpdateItemAsync(BasketUpdateDto item, string UserId)
        {
           
                List<ValidationErorrsDto> errors = new();
                var validator = new BasketUpdateDtoValidator();
                var result = await validator.ValidateAsync(item);

                if (!result.IsValid)
                {
                    errors = result.Errors
                        .Select(e => new ValidationErorrsDto
                        {
                            FieldId = e.PropertyName,
                            Message = e.ErrorMessage
                        }).ToList();

                    return UApiResponderDto<object>.BadRequest(errors);
                }

                await _basketRepository.UpdateItemAsync(item, UserId);

                return UApiResponderDto<object>.Ok(null, "Item updated successfully.");
          
        }

        public async Task<ApiResponseDto<object>> DeleteItemAsync(string ItemId, string UserId)
        {

                await _basketRepository.DeleteItemAsync(ItemId,UserId);

                return UApiResponderDto<object>.Ok(null, "Item deleted successfully.");
        
        }


        public async Task<ApiResponseDto<object>> CheckoutItemAsync(BasketCheckoutDto basketCheckout, int userId)
        {
            var validator = new BasketCheckoutDtoValidator();
            var result = await validator.ValidateAsync(basketCheckout);

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

          
            if (basketCheckout.Items == null || !basketCheckout.Items.Any())
            {
                return UApiResponderDto<object>.BadRequest(new List<ValidationErorrsDto>
        {
            new ValidationErorrsDto
            {
                FieldId = "Items",
                Message = "Basket must contain at least one item."
            }
        });
            }

       
            var totalPrice = basketCheckout.Items
                .Sum(i => i.Price * i.Quantity);

         
            var eventItems = basketCheckout.Items.Select(i => new BasketItemEvent
            {
                ProductId = i.ProductId,
                Name = i.Name,
                Category = i.Category,
                Quantity = i.Quantity,
                ImageUrl = i.ImageUrl,
                TotalPrice = i.Price * i.Quantity
            }).ToList();

         
            var eventMessage = new BasketCheckoutEvent
            {
                UserId = userId,
                Items = eventItems,
                TotalPrice = totalPrice,

                // Billing
                FirstName = basketCheckout.FirstName,
                LastName = basketCheckout.LastName,
                EmailAddress = basketCheckout.EmailAddress,
                AddressLine = basketCheckout.AddressLine,
                Country = basketCheckout.Country,
                State = basketCheckout.State,
                ZipCode = basketCheckout.ZipCode,

                // Payment
                PaymentMethod = basketCheckout.PaymentMethod
            };

         
            await _publishEndpoint.Publish(eventMessage);

           
            await _basketRepository.DeleteBasketAsync(userId.ToString());

            return UApiResponderDto<object>.Ok(null, "Basket checked out successfully.");
        }

    }
}
