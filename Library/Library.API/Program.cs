using Microsoft.EntityFrameworkCore;
using Library.DataAccess;
using Library.DataAccess.Repositories;
using Library.Core.Abstractions;
using Library.API.Extensions;
using Library.Application.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddLibraryMapping();
services.AddDbContext<LibraryDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("LibraryConnection"));
});

services.AddScoped<IBooksRepository, BooksRepository>();
services.AddScoped<IAuthorsRepository, AuthorsRepository>();
services.AddScoped<IBooksService, BooksService>();
services.AddScoped<IAuthorsService, AuthorsService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
