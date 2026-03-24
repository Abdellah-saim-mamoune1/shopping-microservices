using Discount.API.Dtos;
using Discount.API.Repositories;
using Discount.API.Validators;

namespace Discount.API.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }



        public async Task<ApiResponseDto<object>> GetByIdAsync(string Id)
        {

          
            var c = await _discountRepository.GetDiscountById(Id);

            return UApiResponderDto<object>.Ok(c, "Coupon created successfully.");


        }


        public async Task<ApiResponseDto<object>> GetAllAsync()
        {


            var c = await _discountRepository.GetAll();

            return UApiResponderDto<object>.Ok(c, "Coupons fetched successfully.");


        }
        public async Task<ApiResponseDto<object>> CreateAsync(CouponDto coupon)
        {
            
                List<ValidationErorrsDto> errors = new();
                var validator = new CouponSetDtoValidator();
                var result = await validator.ValidateAsync(coupon);

                if (!result.IsValid)
                {
                    errors = result.Errors
                        .Select(e => new ValidationErorrsDto
                        {
                            FieldId = e.PropertyName,
                            Message = e.ErrorMessage
                        }).ToList();

                    return UApiResponderDto<object>.BadRequest(null,"Coupon already exists for this book.");
                }

                if(await _discountRepository.ExistsDiscount(coupon.BookId))
                   return UApiResponderDto<object>.BadRequest();


            var c = await _discountRepository.CreateDiscount(coupon);

                return UApiResponderDto<object>.Ok(c, "Coupon created successfully.");
            
          
        }

        public async Task<ApiResponseDto<object>> UpdateAsync(string id, CouponDto coupon)
        {
           
                List<ValidationErorrsDto> errors = new();

                if (!Guid.TryParse(id, out _))
                {
                    errors.Add(new ValidationErorrsDto
                    {
                        FieldId = "Id",
                        Message = "Invalid coupon Id format."
                    });

                    return UApiResponderDto<object>.BadRequest(errors);
                }

                var validator = new CouponSetDtoValidator();
                var result = await validator.ValidateAsync(coupon);

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

                await _discountRepository.UpdateDiscount(id, coupon);

                return UApiResponderDto<object>.Ok(null, "Coupon updated successfully.");
          
        }

        public async Task<ApiResponseDto<object>> DeleteAsync(string id)
        {
           
                if (!Guid.TryParse(id, out _))
                {
                    var errors = new List<ValidationErorrsDto>
                {
                    new ValidationErorrsDto
                    {
                        FieldId = "Id",
                        Message = "Invalid coupon Id format."
                    }
                };

                    return UApiResponderDto<object>.BadRequest(errors);
                }

                await _discountRepository.DeleteDiscount(id);

                return UApiResponderDto<object>.Ok(null, "Coupon deleted successfully.");
          
        }
    }
}
