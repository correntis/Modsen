using Microsoft.AspNetCore.Http;

namespace Library.Core.Abstractions
{
    public interface IRefreshTokenHandler
    {
        Task<string> HandleUpdateAsync(HttpContext context);
    }
}