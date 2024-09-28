using Library.Core.Abstractions;
using Library.Core.Entities;
using Library.Core.Exceptions;
using Library.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.UseCases.Auth
{
    public class LoginUserUseCase : AuthUseCaseBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginUserUseCase(
            ITokenService tokenService,
            ITokenCacheService tokenCache,
            IUnitOfWork unitOfWork
            )
            : base(tokenService, tokenCache)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(UserEntity, UserToken)> ExecuteAsync(string email, string password)
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
    }
}
