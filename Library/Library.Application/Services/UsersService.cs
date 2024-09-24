using Library.Core.Abstractions;
using Library.Core.Exceptions;
using Library.Core.Models;
using System;

namespace Library.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<Guid> AddAsync(User user)
        {
            return await _usersRepository.AddAsync(user);
        }

        public async Task<Guid> AddBookAsync(Guid userId, Guid bookId)
        {
            var guid = await _usersRepository.AddBookAsync(userId, bookId, DateTime.UtcNow, DateTime.UtcNow.AddDays(7));

            ThrowNotFoundIfEmptyGuid(guid);

            return guid;
        }

        public async Task<Guid> AddRolesAsync(Guid userId, string[] rolesNames)
        {
            var guid = await _usersRepository.AddRolesAsync(userId, rolesNames);

            ThrowNotFoundIfEmptyGuid(guid);

            return guid;
        }

        public async Task<Guid> UpdateAsync(User user)
        {
            var guid = await _usersRepository.UpdateAsync(user);

            ThrowNotFoundIfEmptyGuid(guid);

            return guid;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var guid = await _usersRepository.DeleteAsync(id);

            ThrowNotFoundIfEmptyGuid(guid);

            return guid;
        }

        public async Task<Guid> DeleteBookAsync(Guid userId, Guid bookId)
        {
            var guid = await _usersRepository.DeleteBookAsync(userId, bookId);

            ThrowNotFoundIfEmptyGuid(guid);

            return guid;
        }

        public async Task<User> GetAsync(Guid id)
        {
            var user =  await _usersRepository.GetAsync(id);

            ThrowNotFoundIfUserIsNull(user);

            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _usersRepository.GetByEmailAsync(email);

            ThrowNotFoundIfUserIsNull(user);

            return user;
        }

        private void ThrowNotFoundIfEmptyGuid(Guid guid)
        {
            if(guid == Guid.Empty)
            {
                throw new NotFoundException("User not found.");
            }
        }

        private void ThrowNotFoundIfUserIsNull(User author)
        {
            if(author is null)
            {
                throw new NotFoundException("User not found.");
            }
        }
    }
}
