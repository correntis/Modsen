using Library.Core.Abstractions;
using Library.Core.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<JwtOptions> _jwtOptions;

        public TokenService(
                IOptions<JwtOptions> jwtOptions
            )
        {
            _jwtOptions = jwtOptions;
        }

        public string CreateAccessToken(Guid userId, List<string> userRoles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Secret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtClaims = new List<Claim>()
            {
                new("User_Id", userId.ToString()),
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            };

            foreach(var role in userRoles)
            {
                jwtClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expiresTime = DateTime.UtcNow.AddHours(12);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Value.Issuer,
                audience: _jwtOptions.Value.Audience,
                claims: jwtClaims,
                expires: expiresTime,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string CreateRefreshToken()
        {
            var tokenLength = 64;
            var characters = new char[] {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
                'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '_', '-', '.',
                '!','@','#','$','%','^','&','*','(',')','+','='
            };

            var numberOfChar = characters.Length;

            Random rand = new();
            string token = string.Join("",
                Enumerable
                    .Range(0, tokenLength)
                    .Select(x => characters[rand.Next(numberOfChar)])
            );

            return token;
        }
    }
}
