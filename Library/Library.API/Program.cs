using Microsoft.EntityFrameworkCore;
using Library.DataAccess;
using Library.DataAccess.Repositories;
using Library.Core.Abstractions;
using Library.API.Extensions;
using Library.Application.Services;
using Library.Core.Configuration;
using Library.API.Middleware;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

if(configuration["IS_MIGRATION"] == "false") // Wait migration container to start
{
    await Task.Delay(20000);
}
else // Wait for database container to start
{
    await Task.Delay(2000);
}

configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

services.ConfigureOptions<JwtOptions>(configuration);

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();


services.AddMapping();
services.AddDbContext<LibraryDbContext>(options =>
{
    options.UseNpgsql(
        $"User ID={configuration["POSTGRES_USER"]};" +
        $"Password={configuration["POSTGRES_PASSWORD"]};" +
        $"Host={configuration["POSTGRES_HOST"]};" +
        $"Port={configuration["POSTGRES_PORT"]};" +
        $"Database={configuration["POSTGRES_DB"]}"
    );
});

services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = $"{configuration["TOKENS_STORAGE_HOST"]}:{configuration["TOKENS_STORAGE_PORT"]}";
    options.InstanceName = "Library";
});

services.AddAuthentication(configuration);

services.AddUseCases();
services.AddRepositories();
services.AddValidators();

services.AddScoped<ITokenService, TokenService>();
services.AddScoped<ITokenCacheService, TokenCacheService>();
services.AddScoped<IFileService, FileService>();
services.AddScoped<IRefreshTokenHandler, RefreshTokenHandler>();

services.AddScoped<ExceptionMiddleware>();
services.AddScoped<AuthenticationMiddleware>();

var app = builder.Build();

using(var scope = app.Services.CreateScope()) // Seed default data
{
    if(configuration["SEED_ON_START"] == "true")
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

        await LibraryDbInitializer.InitializeAsync(dbContext);
    }
}

app.UseCors(options =>
{
    options
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

app.AddStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
