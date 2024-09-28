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
    public class RegisterUserUseCase : AuthUseCaseBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserUseCase(
            ITokenService tokenService,
            ITokenCacheService tokenCache,
            IUnitOfWork unitOfWork
            )
            : base(tokenService, tokenCache)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserToken> ExecuteAsync(string username, string email, string password)
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
    }
}
