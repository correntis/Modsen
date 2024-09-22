using FluentValidation;
using Library.API.Contracts;
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
                .MinimumLength(User.MIN_PASSWORD_LENGTH)
                .MaximumLength(User.MAX_PASSWORD_LENGTH)
                .WithMessage("Password to short.");
            
            RuleFor(l => l.UserName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(User.MAX_USERNAME_LENGTH); 
        }
    }
}
