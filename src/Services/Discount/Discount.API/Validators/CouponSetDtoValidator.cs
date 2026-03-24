using Discount.API.Dtos;
using Discount.API.Entities;
using FluentValidation;

namespace Discount.API.Validators
{

    public class CouponSetDtoValidator : AbstractValidator<CouponDto>
    {
        public CouponSetDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Coupon name is required.")
                .MaximumLength(100).WithMessage("Coupon name must not exceed 100 characters.");

            RuleFor(x => x.BookId)
                .NotEmpty().WithMessage("BookId is required.")
                .MaximumLength(50).WithMessage("BookId must not exceed 500 characters.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Amount cannot exceed 100.");
        }
    }
}