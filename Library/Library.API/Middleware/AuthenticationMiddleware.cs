
using Library.Core.Abstractions;
using System.IdentityModel.Tokens.Jwt;

namespace Library.API.Middleware
{
    public class AuthenticationMiddleware : IMiddleware
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenCacheService _tokenCacheService;
        private readonly IRefreshTokenHandler _refreshTokenHandler;

        public AuthenticationMiddleware(
            ITokenService tokenService,
            ITokenCacheService tokenCacheService,
            IRefreshTokenHandler refreshTokenHandler
            )
        {
            _tokenService = tokenService;
            _tokenCacheService = tokenCacheService;
            _refreshTokenHandler = refreshTokenHandler;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if(context.Request.Cookies.TryGetValue("accessToken", out var accessToken))
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

                if(jwtToken.ValidTo < DateTime.UtcNow)
                {
                    var newAccessToken = _refreshTokenHandler.HandleUpdateAsync(context).Result;
                    if(!string.IsNullOrEmpty(newAccessToken))
                    {
                        accessToken = newAccessToken;
                    }
                }

                context.Request.Headers.Authorization = "Bearer " + accessToken;
            }

            await next(context);
        }
    }
}
