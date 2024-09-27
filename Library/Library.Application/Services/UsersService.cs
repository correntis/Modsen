using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Entities;
using Library.Core.Exceptions;
using Library.Core.Models;
using System;

namespace Library.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersService(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> AddAsync(User user)
        {
            var userEntity = _mapper.Map<UserEntity>(user);

            await _unitOfWork.UsersRepository.AddAsync(userEntity);
            await _unitOfWork.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task UpdateAsync(User user)
        {
            var userEntity = await _unitOfWork.UsersRepository.GetAsync(user.Id);
            ThrowNotFoundIfNull(userEntity);

            userEntity.UserName = user.UserName;
            userEntity.Email = user.Email;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var userEntity = await _unitOfWork.UsersRepository.GetAsync(id);
            ThrowNotFoundIfNull(userEntity);

            _unitOfWork.UsersRepository.Delete(userEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<UserEntity> GetAsync(Guid id)
        {
            var userEntity = await _unitOfWork.UsersRepository.GetAsync(id);
            ThrowNotFoundIfNull(userEntity);

            return userEntity;
        }

        public async Task<UserEntity> GetByEmailAsync(string email)
        {
            var userEntity = await _unitOfWork.UsersRepository.GetByEmailAsync(email);
            ThrowNotFoundIfNull(userEntity);

            return userEntity;
        }

        public async Task IssueBookAsync(Guid userId, Guid bookId)
        {
            var bookEntity = await _unitOfWork.BooksRepository.GetAsync(bookId);
            ThrowNotFoundIfNull(bookEntity);

            var userEntity = await _unitOfWork.UsersRepository.GetAsync(userId);
            ThrowNotFoundIfNull(userEntity);

            bookEntity.TakenAt = DateTime.UtcNow;
            bookEntity.ReturnBy = DateTime.UtcNow.AddDays(7);
            userEntity.Books.Add(bookEntity);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(Guid userId, Guid bookId)
        {
            var userEntity = await _unitOfWork.UsersRepository.GetAsync(userId);
            ThrowNotFoundIfNull(userEntity);

            var bookEntity = await _unitOfWork.BooksRepository.GetAsync(bookId);
            ThrowNotFoundIfNull(bookEntity);

            bookEntity.TakenAt = DateTime.MinValue;
            bookEntity.ReturnBy = DateTime.MinValue;
            userEntity.Books.Remove(bookEntity);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddRolesAsync(Guid userId, string[] userRoles)
        {
            var userEntity = await _unitOfWork.UsersRepository.GetAsync(userId);
            ThrowNotFoundIfNull(userEntity);

            await _unitOfWork.UsersRepository.AddRolesAsync(userEntity, userRoles);
            await _unitOfWork.SaveChangesAsync();
        }


        private void ThrowNotFoundIfNull<T>(T entity)
        {
            if(entity is null)
            {
                throw new NotFoundException();
            }
        }
    }
}
