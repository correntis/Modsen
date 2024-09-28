using Library.Core.Abstractions;
using Library.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Auth
{
    public abstract class AuthUseCaseBase
    {
        protected readonly ITokenService _tokenService;
        protected readonly ITokenCacheService _tokenCache;

        protected AuthUseCaseBase(
            ITokenService tokenService,
            ITokenCacheService tokenCache
            )
        {
            _tokenService = tokenService;
            _tokenCache = tokenCache;
        }

        protected async Task<UserToken> CreateTokensAsync(Guid userId, string[] roles)
        {
            var accessToken = _tokenService.CreateAccessToken(userId, roles);
            var refreshToken = _tokenService.CreateRefreshToken();

            await _tokenCache.SetTokenAsync(refreshToken, userId.ToString(), DateTime.UtcNow.AddDays(14));

            return new UserToken()
            {
                UserId = userId,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
    }
}
