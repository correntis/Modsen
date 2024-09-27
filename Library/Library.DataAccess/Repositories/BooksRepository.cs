using Library.Core.Abstractions;
using Library.Core.Models;
using Library.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly LibraryDbContext _context;

        public BooksRepository(
            LibraryDbContext context
            )
        {
            _context = context;
        }

        public async Task AddAsync(BookEntity book)
        {
            await _context.Books.AddAsync(book);
        }

        public void Delete(BookEntity book)
        {
            _context.Books.Remove(book);
        }

        public async Task<BookEntity> GetAsync(Guid id)
        {
            return await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<BookEntity> GetByIsbnAsync(string isbn)
        {
            return await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<BookEntity> GetByAuthorAsync(Guid authorId)
        {
            return await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Authors.Any(a => a.Id == authorId));
        }

        public async Task<IEnumerable<BookEntity>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Authors)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<BookEntity>> GetPageAsync(int pageIndex, int pageSize, BooksFilter filter)
        {
            var query = _context.Books.AsQueryable();

            query = query.Include(b => b.Authors);

            BuildFilteredQuery(ref query, filter);

            query = query.AsNoTracking()
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<int> GetAmountAsync(BooksFilter filter)
        {
            var query = _context.Books.Include(b => b.Authors).AsQueryable();

            BuildFilteredQuery(ref query, filter);

            return await query.CountAsync();
        }

        private void BuildFilteredQuery(ref IQueryable<BookEntity> query, BooksFilter filter)
        {
            if(!string.IsNullOrEmpty(filter.Author))
            {
                query = query.Where(b => b.Authors.Any(a => a.Name.ToLower().Contains(filter.Author.ToLower())));
            }

            if(!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(b =>
                    b.Name.ToLower().Contains(filter.Name.ToLower()) || b.Name.ToLower().Contains(filter.Name.ToLower())
                    );
            }

            if(!string.IsNullOrEmpty(filter.Genre))
            {
                query = query.Where(b => b.Genre.ToLower().Contains(filter.Genre.ToLower()));
            }
        }
    }
}
