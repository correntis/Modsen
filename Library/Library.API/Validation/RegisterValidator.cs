using FluentValidation;
using Library.API.Contracts;
using Library.Core.Constants;
using Library.Core.Models;

namespace Library.API.Validation
{
    public class RegisterValidator : AbstractValidator<RegisterContract>
    {
        public RegisterValidator()
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
                .MaximumLength(UserConstants.MAX_PASSWORD_LENGTH)
                .WithMessage("Password to short.");
            
            RuleFor(l => l.UserName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(UserConstants.MAX_USERNAME_LENGTH); 
        }
    }
}
