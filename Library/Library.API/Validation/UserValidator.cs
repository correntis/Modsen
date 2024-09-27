using FluentValidation;
using Library.API.Contracts;
using Library.Core.Constants;
using Library.Core.Models;

namespace Library.API.Validation
{
    public class UserValidator : AbstractValidator<UserContract>
    {
        public UserValidator()
        {
            RuleFor(l => l.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Incorrect email.");

            RuleFor(l => l.UserName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(UserConstants.MAX_USERNAME_LENGTH);
        }
    }
}
