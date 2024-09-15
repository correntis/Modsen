using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<BooksRepository> _logger;
        private readonly IMapper _mapper;

        public UsersRepository(
            LibraryDbContext context,
            ILogger<BooksRepository> logger,
            IMapper mapper,
            IBooksRepository booksRepository
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Guid> AddAsync(User user)
        {
            var userEntity = _mapper.Map<UserEntity>(user);

            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task<Guid> AddBookAsync(Guid userId, Guid bookId)
        {
            var bookEntity = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if(bookEntity is null)
            {
                return Guid.Empty;
            }

            var userEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if(userEntity is null)
            {
                return Guid.Empty;
            }

            userEntity.Books.Add(bookEntity);
            await _context.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task<Guid> AddRolesAsync(Guid userId, string[] userRoles)
        {
            var userEntity = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var userRolesEntities = await _context.UserRoles
                .Where(ur => userRoles.Any(role => role == ur.Name))
                .ToListAsync();

            foreach (var role in userRolesEntities)
            {
                userEntity.Roles.Add(role);
            }

            await _context.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task<Guid> UpdateAsync(User user)
        {
            var userEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if(userEntity is null)
            {
                return Guid.Empty;
            }

            userEntity.UserName = user.UserName;
            userEntity.Email = user.Email;

            await _context.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var userEntity = await _context.Users
                .Include(u => u.Books)
                .FirstOrDefaultAsync(u => u.Id == id);

            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task<Guid> DeleteBookAsync(Guid userId, Guid bookId)
        {
            var bookEntity = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if(bookEntity is null)
            {
                return Guid.Empty;
            }

            var userEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if(userEntity is null)
            {
                return Guid.Empty;
            }

            userEntity.Books.Remove(bookEntity);
            await _context.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task<User> GetAsync(Guid id)
        {
            var userEntity = await _context.Users
                .Include(u => u.Books)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if(userEntity is null)
            {
                return null;
            }

            return _mapper.Map<User>(userEntity);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var userEntity = await _context.Users
                .Include(u => u.Roles)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            if(userEntity is null)
            {
                return null;
            }

            return _mapper.Map<User>(userEntity);
        }
    }
}
