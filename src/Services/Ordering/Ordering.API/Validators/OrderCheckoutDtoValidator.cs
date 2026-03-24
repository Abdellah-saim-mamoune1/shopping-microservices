using FluentValidation;
using Ordering.API.Dtos;
using System.Text.RegularExpressions;

namespace Ordering.API.Validators
{
    public class OrderCheckoutDtoValidator : AbstractValidator<OrderCheckoutDto>
    {
        public OrderCheckoutDtoValidator()
        {

            // Billing Address

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100);

            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.AddressLine)
                .NotEmpty().WithMessage("AddressLine is required.")
                .MaximumLength(250);

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(100);

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required.")
                .MaximumLength(100);

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("ZipCode is required.")
                .MaximumLength(20);


            // Payment

            RuleFor(x => x.PaymentMethod)
                .GreaterThan(0).WithMessage("PaymentMethod is required.");
        }

   
    }
}
