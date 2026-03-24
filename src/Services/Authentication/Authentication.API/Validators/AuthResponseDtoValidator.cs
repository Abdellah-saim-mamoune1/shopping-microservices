using Authentication.API.Dtos;
using FluentValidation;

namespace Authentication.API.Validators;

public class AuthResponseDtoValidator
    : AbstractValidator<AuthResponseDto>
{
    public AuthResponseDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Role)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.AccessToken)
            .NotEmpty();

        RuleFor(x => x.RefreshToken)
            .NotEmpty();

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Expiration date must be in the future.");
    }
}