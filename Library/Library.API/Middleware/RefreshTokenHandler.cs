using Library.Core.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Library.API.Middleware
{
    public class RefreshTokenHandler : IRefreshTokenHandler
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenCacheService _tokenCacheService;

        public RefreshTokenHandler(
            ITokenService tokenService,
            ITokenCacheService tokenCacheService
            )
        {
            _tokenService = tokenService;
            _tokenCacheService = tokenCacheService;
        }
        public async Task<string> HandleUpdateAsync(HttpContext context)
        {
            if(context.Request.Cookies.ContainsKey("refreshToken"))
            {
                var refreshToken = context.Request.Cookies["refreshToken"];
                var userIdString = await _tokenCacheService.GetValue(refreshToken);

                if(string.IsNullOrEmpty(userIdString))
                {
                    return null;
                }

                var userId = Guid.Parse(userIdString);
                var userRoleClaims = GetUserRoles(context.Request.Cookies["accessToken"]);

                var accessToken = _tokenService.CreateAccessToken(userId, userRoleClaims);

                UpdateAccessToken(accessToken, context);

                return accessToken;
            }

            return null;
        }

        private string[] GetUserRoles(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);

            return token.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToArray();
        }

        private void UpdateAccessToken(string accessToken, HttpContext context)
        {
            context.Response.Cookies.Append("accessToken", accessToken,
                new CookieOptions() { HttpOnly = true, Expires = DateTime.UtcNow.AddMonths(1) }
            );
        }
    }
}
