using FluentValidation;
using Library.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace Library.API.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
				await next(context);
			}
            catch(ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                WriteResponseErrorAsync(context, ex);
            }
            catch(NotFoundException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                WriteResponseErrorAsync(context, ex);
            }
            catch(EntityAlreadyExistsException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                WriteResponseErrorAsync(context, ex);
            }
            catch(Exception ex)
			{
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                WriteResponseErrorAsync(context, ex);
            }
        }

        private async void WriteResponseErrorAsync(HttpContext context, Exception ex)
        {
            _logger.LogError("Error: {value}", ex.ToString());

            context.Response.ContentType = "application/json";

            var response = new ApiException(context.Response.StatusCode, ex.Message, ex.ToString());

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
