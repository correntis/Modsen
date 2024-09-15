using Microsoft.EntityFrameworkCore;
using Library.DataAccess;
using Library.DataAccess.Repositories;
using Library.Core.Abstractions;
using Library.API.Extensions;
using Library.Application.Services;
using Library.Core.Configuration;

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


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
