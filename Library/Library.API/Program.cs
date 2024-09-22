using Microsoft.EntityFrameworkCore;
using Library.DataAccess;
using Library.DataAccess.Repositories;
using Library.Core.Abstractions;
using Library.API.Extensions;
using Library.Application.Services;
using Library.Core.Configuration;
using Microsoft.Extensions.FileProviders;
using Library.API.Middleware;
using FluentValidation;
using Library.API.Contracts;
using Library.API.Validation;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

services.ConfigureOptions<JwtOptions>(configuration);

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();


services.AddLibraryMapping();
services.AddDbContext<LibraryDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("LibraryConnection"));
});

services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration["RedisConnection"];
    options.InstanceName = "Library";
});

services.AddLibraryAuthentication(configuration);

services.AddScoped<IBooksRepository, BooksRepository>();
services.AddScoped<IAuthorsRepository, AuthorsRepository>();
services.AddScoped<IUsersRepository, UsersRepository>();

services.AddScoped<IBooksService, BooksService>();
services.AddScoped<IAuthorsService, AuthorsService>();
services.AddScoped<IUsersService, UsersService>();
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<ITokenService, TokenService>();
services.AddScoped<ITokenCacheService, TokenCacheService>();
services.AddScoped<IFileService, FileService>();
services.AddScoped<IRefreshTokenHandler, RefreshTokenHandler>();
services.AddScoped<ExceptionMiddleware>();

services.AddScoped<IValidator<UserContract>, UserValidator>();
services.AddScoped<IValidator<BookContract>, BookValidator>();
services.AddScoped<IValidator<AuthorContract>, AuthorValidator>();
services.AddScoped<IValidator<LoginContract>, LoginValidator>();
services.AddScoped<IValidator<RegisterContract>, RegisterValidator>();


var app = builder.Build();

app.UseCors(options =>
{
    options
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

app.AddLibraryStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionMiddleware>();

app.Run();
