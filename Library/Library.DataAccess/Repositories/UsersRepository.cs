using Library.Core.Abstractions;
using Library.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly LibraryDbContext _context;

        public UsersRepository(
            LibraryDbContext context
            )
        {
            _context = context;
        }

        public async Task AddAsync(UserEntity user)
        {
           await _context.Users.AddAsync(user);
        }

        public async Task AddRolesAsync(UserEntity entity, string[] userRoles)
        {
            var userRolesEntities = await _context.UserRoles
                .Where(ur => userRoles.Any(role => role == ur.Name))
                .ToListAsync();

            foreach (var role in userRolesEntities)
            {
                entity.Roles.Add(role);
            }
        }

        public void Delete(UserEntity entity)
        {
            _context.Users.Remove(entity);
        }

        public async Task<UserEntity> GetAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Books)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserEntity> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Books)
                .FirstOrDefaultAsync(u => u.Email == email);

        }
    }
}
