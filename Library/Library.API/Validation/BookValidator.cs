using FluentValidation;
using Library.API.Contracts;
using Library.Core.Constants;
using Library.Core.Models;

namespace Library.API.Validation
{
    public class BookValidator : AbstractValidator<BookContract>
    {
        public BookValidator()
        {
            RuleFor(b => b.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(BookConstants.MAX_NAME_LENGTH)
                .WithMessage("Name too long");

            RuleFor(b => b.Genre)
                .NotNull()
                .NotEmpty()
                .MaximumLength(BookConstants.MAX_GENRE_LENGTH)
                .WithMessage("Genre too long");

            RuleFor(b => b.Description)
                .NotNull()
                .NotEmpty()
                .MaximumLength(BookConstants.MAX_DESCRIPTION_LENGTH)
                .WithMessage("Description too long");

            RuleFor(b => b.ISBN)
                .NotNull()
                .NotEmpty()
                .MaximumLength(BookConstants.MAX_ISBN_LENGTH)
                .WithMessage("ISBN too long");
        }
    }
}
