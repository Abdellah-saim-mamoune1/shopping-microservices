using Basket.API.Dtos;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Basket.API.Validators
{
    public class BasketCheckoutDtoValidator : AbstractValidator<BasketCheckoutDto>
    {
        public BasketCheckoutDtoValidator()
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

            RuleFor(x => x.CardName)
                .NotEmpty().WithMessage("Card name is required.")
                .MaximumLength(100);

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required.")
                .CreditCard().WithMessage("Invalid card number format.");

            RuleFor(x => x.Expiration)
                .NotEmpty().WithMessage("Expiration date is required.")
                .Must(BeValidExpiration).WithMessage("Expiration must be in MM/YY format and not expired.");

            RuleFor(x => x.CVV)
                .NotEmpty().WithMessage("CVV is required.")
                .Matches(@"^\d{3,4}$")
                .WithMessage("CVV must be 3 or 4 digits.");

            RuleFor(x => x.PaymentMethod)
                .GreaterThan(0).WithMessage("PaymentMethod is required.");
        }

     
        private bool BeValidExpiration(string expiration)
        {
            if (!Regex.IsMatch(expiration, @"^(0[1-9]|1[0-2])\/\d{2}$"))
                return false;

            var parts = expiration.Split('/');
            var month = int.Parse(parts[0]);
            var year = 2000 + int.Parse(parts[1]);

            var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            return lastDayOfMonth >= DateTime.UtcNow.Date;
        }
    }
}
