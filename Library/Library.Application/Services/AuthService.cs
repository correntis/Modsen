using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Entities;
using Library.Core.Exceptions;
using Library.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Library.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly ITokenCacheService _tokenCache;
        private readonly IMapper _mapper;

        public AuthService(
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            ITokenCacheService tokenCache,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _tokenCache = tokenCache;
            _mapper = mapper;
        }

        public async Task<(UserEntity, UserToken)> Login(string email, string password)
        {
            var userEntity = await _unitOfWork.UsersRepository.GetByEmailAsync(email)
                ?? throw new NotFoundException();

            var passwordHasher = new PasswordHasher<User>();
            var verifyResult = passwordHasher.VerifyHashedPassword(null, userEntity.PasswordHash, password);
            if(verifyResult == PasswordVerificationResult.Failed)
            {
                throw new InvalidPasswordException("Invalid password.");
            }

            var tokens = await CreateTokensAsync(userEntity.Id, userEntity.Roles.Select(ur => ur.Name).ToArray());

            return (userEntity, tokens);
        }

        public async Task<UserToken> Register(string username, string email, string password)
        {
            var existedUser = await _unitOfWork.UsersRepository.GetByEmailAsync(email)
                ?? throw new EntityAlreadyExistsException();

            var defaultRoles = new string[] { "User" };
            var userEntity = new UserEntity()
            {
                UserName = username,
                Email = email,
                PasswordHash = new PasswordHasher<UserEntity>().HashPassword(null, password),
            };

            await _unitOfWork.UsersRepository.AddAsync(userEntity);
            await _unitOfWork.UsersRepository.AddRolesAsync(userEntity, defaultRoles);
            await _unitOfWork.SaveChangesAsync();

            return await CreateTokensAsync(userEntity.Id, defaultRoles);
        }

        private async Task<UserToken> CreateTokensAsync(Guid userId, string[] roles)
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
