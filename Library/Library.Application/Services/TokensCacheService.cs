using Library.Core.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace Library.Application.Services
{
    public class TokenCacheService : ITokenCacheService
    {
        private readonly IDistributedCache _cache;

        public TokenCacheService(
            IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetTokenAsync(string token, string userId, DateTimeOffset duration)
        {
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(duration);

            await _cache.SetStringAsync(token, userId.ToString(), options);
        }

        public async Task RemoveTokenAsync(string oldToken)
        {
            await _cache.RemoveAsync(oldToken);
        }

        public async Task<string> GetValue(string token)
        {
            return await _cache.GetStringAsync(token);
        }
    }
}
