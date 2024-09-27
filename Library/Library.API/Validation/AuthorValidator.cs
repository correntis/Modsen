using FluentValidation;
using Library.API.Contracts;
using Library.Core.Constants;
using Library.Core.Models;

namespace Library.API.Validation
{
    public class AuthorValidator : AbstractValidator<AuthorContract>
    {
        public AuthorValidator()
        {
            RuleFor(a => a.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(AuthorConstants.MAX_NAME_LENGTH)
                .WithMessage("Name too long.");

            RuleFor(a => a.Surname)
                .NotNull()
                .NotEmpty()
                .MaximumLength(AuthorConstants.MAX_SURNAME_LENGTH)
                .WithMessage("Surname too long.");

            RuleFor(a => a.Country)
                .NotNull()
                .NotEmpty()
                .MaximumLength(AuthorConstants.MAX_COUNTRY_LENGTH)
                .WithMessage("Country too long.");

            RuleFor(a => a.Birthday)
                .NotNull()
                .NotEmpty()
                .LessThan(DateTime.UtcNow)
                .WithMessage("Date must be less then now.");
        }
    }
}
