using Basket.API.Dtos;
using FluentValidation;

namespace Basket.API.Validators
{
    public class BasketUpdateDtoValidator : AbstractValidator<BasketUpdateDto>
    {
        public BasketUpdateDtoValidator()
        {
         

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.")
                .MaximumLength(100);

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(1000)
                .WithMessage("Quantity cannot exceed 1000.");
        }
    }

}
