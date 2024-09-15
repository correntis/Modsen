
namespace Library.Core.Abstractions
{
    public interface ITokenCacheService
    {
        Task<string> GetValue(string token);
        Task RemoveTokenAsync(string oldToken);
        Task SetTokenAsync(string token, string userId, DateTimeOffset duration);
    }
}