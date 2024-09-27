using Library.Core.Entities;
using Library.Core.Models;

namespace Library.Core.Abstractions
{
    public interface IUsersRepository
    {
        Task AddAsync(UserEntity user);
        Task AddRolesAsync(UserEntity userEntity, string[] userRoles);
        void Delete(UserEntity user);

        Task<UserEntity> GetAsync(Guid id);
        Task<UserEntity> GetByEmailAsync(string email);
    }
}