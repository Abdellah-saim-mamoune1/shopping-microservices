using Catalog.Dtos;
using FluentValidation;

namespace Catalog.API.Validators
{
    public class BookDtoValidator : AbstractValidator<BookDto>
    {
        public BookDtoValidator()
        {

            RuleFor(b => b.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(150).WithMessage("Name must not exceed 150 characters.");

            RuleFor(b => b.Category)
                .NotEmpty().WithMessage("Category is required.")
                .MaximumLength(100).WithMessage("Category must not exceed 100 characters.");

            RuleFor(b => b.Summary)
                .MaximumLength(500).WithMessage("Summary must not exceed 500 characters.");

            RuleFor(b => b.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(b => b.Authors)
                .NotEmpty().WithMessage("At least one author is required.");

            RuleForEach(b => b.Authors)
                .NotEmpty().WithMessage("Author name cannot be empty.")
                .MaximumLength(100).WithMessage("Author name must not exceed 100 characters.");

            RuleFor(b => b.ImageUrl)
                .Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("ImageUrl must be a valid absolute URL.");

            RuleFor(b => b.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(b => b.PagesCount)
                .GreaterThan(0).WithMessage("PagesCount must be greater than 0.");

            RuleFor(b => b.averageRating)
                .InclusiveBetween(0, 5).WithMessage("AverageRating must be between 0 and 5.");

            RuleFor(b => b.ratingsCount)
                .GreaterThanOrEqualTo(0).WithMessage("RatingsCount cannot be negative.");

            RuleFor(b => b.PublishedAt)
                .Must(date => !date.HasValue || date.Value <= DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("PublishedAt cannot be in the future.");

        }
    }
}
