using Library.Core.Abstractions;
using Library.Core.Exceptions;
using Library.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Library.Application.Services
{
    public class AuthService : IAuthService
    { 
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenService _tokenService;
        private readonly ITokenCacheService _tokenCache;

        public AuthService(
            IUsersRepository usersRepository,
            ITokenService tokenService,
            ITokenCacheService tokenCache
            )
        {
            _usersRepository = usersRepository;
            _tokenService = tokenService;
            _tokenCache = tokenCache;
        }

        public async Task<(User, UserToken)> Login(string email, string password)
        {
            var user = await _usersRepository.GetByEmailAsync(email) 
                ?? throw new NotFoundException("User not found.");
            
            var passwordHasher = new PasswordHasher<User>();
            if(passwordHasher.VerifyHashedPassword(null, user.PasswordHash, password) == PasswordVerificationResult.Failed)
            {
                throw new InvalidPasswordException("Invalid password.");
            }

            return (user, await CreateTokensAsync(user.Id, user.Roles.Select(ur => ur.Name).ToList()));
        }

        public async Task<UserToken> Register(string username, string email, string password)
        {
            var user = new User
            {
                UserName = username,
                Email = email,
                PasswordHash = new PasswordHasher<User>().HashPassword(null, password),
            };

            _ = await _usersRepository.GetByEmailAsync(email)
                ?? throw new EntityAlreadyExistsException("User already exists.");

            var guid = await _usersRepository.AddAsync(user);
            await _usersRepository.AddRolesAsync(guid, ["User"]);

            return await CreateTokensAsync(guid, ["User"]);
        }

        private async Task<UserToken> CreateTokensAsync(Guid userId, List<string> roles)
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
