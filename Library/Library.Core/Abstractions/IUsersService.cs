using Library.Core.Entities;
using Library.Core.Models;

namespace Library.Core.Abstractions
{
    public interface IUsersService
    {
        Task<Guid> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);

        Task<UserEntity> GetAsync(Guid id);
        Task<UserEntity> GetByEmailAsync(string email);

        Task AddRolesAsync(Guid userId, string[] userRoles);

        Task IssueBookAsync(Guid userId, Guid bookId);
        Task DeleteBookAsync(Guid userId, Guid bookId);
    }
}