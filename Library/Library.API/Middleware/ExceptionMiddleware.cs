
using Library.API.Exceptions;
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
			catch(Exception ex)
			{
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.ToString())
                    : new ApiException(context.Response.StatusCode, "Internal Server Error");

                var json = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
			}
        }
    }
}
