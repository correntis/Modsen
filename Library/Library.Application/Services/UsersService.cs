using Library.Core.Abstractions;
using Library.Core.Models;

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
            return await _usersRepository.AddBookAsync(userId, bookId, DateTime.UtcNow, DateTime.UtcNow.AddDays(7));
        }

        public async Task<Guid> AddRolesAsync(Guid userId, string[] rolesNames)
        {
            return await _usersRepository.AddRolesAsync(userId, rolesNames);
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await _usersRepository.DeleteAsync(id);
        }

        public async Task<Guid> DeleteBookAsync(Guid userId, Guid bookId)
        {
            return await _usersRepository.DeleteBookAsync(userId, bookId);
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await _usersRepository.GetAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _usersRepository.GetByEmailAsync(email);
        }

        public async Task<Guid> UpdateAsync(User user)
        {
            return await _usersRepository.UpdateAsync(user);
        }
    }
}
