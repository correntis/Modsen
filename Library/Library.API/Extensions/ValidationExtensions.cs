using FluentValidation;
using Library.API.Contracts;
using Library.API.Validation;

namespace Library.API.Extensions
{
    public static class ValidationExtensions
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<UserContract>, UserValidator>();
            services.AddScoped<IValidator<BookContract>, BookValidator>();
            services.AddScoped<IValidator<AuthorContract>, AuthorValidator>();
            services.AddScoped<IValidator<LoginContract>, LoginValidator>();
            services.AddScoped<IValidator<RegisterContract>, RegisterValidator>();
        }
    }
}
