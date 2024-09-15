using Library.Core.Models;

namespace Library.Core.Abstractions
{
    public interface IUsersRepository
    {
        Task<Guid> AddAsync(User user);
        Task<Guid> AddBookAsync(Guid userId, Guid bookId);
        Task<Guid> AddRolesAsync(Guid userId, string[] roleNames);
        Task<Guid> DeleteAsync(Guid id);
        Task<Guid> DeleteBookAsync(Guid userId, Guid bookId);
        Task<User> GetAsync(Guid id);
        Task<User> GetByEmailAsync(string email);
        Task<Guid> UpdateAsync(User user);
    }
}