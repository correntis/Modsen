using Library.Core.Abstractions;
using Library.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories
{
    public class AuthorsRepository : IAuthorsRepository
    {
        private readonly LibraryDbContext _context;

        public AuthorsRepository(
            LibraryDbContext context
            )
        {
            _context = context;
        }

        public async Task AddAsync(AuthorEntity author)
        {
            await _context.Authors.AddAsync(author);
        }

        public void Delete(AuthorEntity author)
        {
            _context.Authors.Remove(author);
        }

        public async Task<AuthorEntity> GetAsync(Guid id)
        {
            return await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<AuthorEntity>> GetAllAsync()
        {
            return await _context.Authors
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<AuthorEntity>> GetPageAsync(int pageIndex, int pageSize)
        {
            return await _context.Authors
                .AsNoTracking()
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
