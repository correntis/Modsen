using FluentValidation;
using Library.API.Contracts;
using Library.Core.Constants;
using Library.Core.Models;

namespace Library.API.Validation
{
    public class LoginValidator : AbstractValidator<LoginContract>
    {
        public LoginValidator()
        {
            RuleFor(l => l.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Incorrect email.");

            RuleFor(l => l.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(UserConstants.MIN_PASSWORD_LENGTH)
                .WithMessage("Password to short.");
        }
    }
}
