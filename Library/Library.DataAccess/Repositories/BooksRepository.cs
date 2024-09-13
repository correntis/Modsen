using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.DataAccess.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<BooksRepository> _logger;
        private readonly IMapper _mapper;

        public BooksRepository(
            LibraryDbContext context,
            ILogger<BooksRepository> logger,
            IMapper _mapper
            )
        {
            _context = context;
            _logger = logger;
            this._mapper = _mapper;
        }

        public async Task<Guid> AddAsync(Book book)
        {
            var bookEntity = _mapper.Map<BookEntity>(book);

            _context.Books.Add(bookEntity);
            await _context.SaveChangesAsync();

            return bookEntity.Id;
        }

        public async Task<Guid> AddAuthorAsync(Guid bookId, Guid authorId)
        {
            var bookEntity = await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if(bookEntity is null)
            {
                return Guid.Empty;
            }

            var authorEntity = await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == authorId);

            if(authorEntity is null)
            {
                return Guid.Empty;
            }

            bookEntity.Authors.Add(authorEntity);
            await _context.SaveChangesAsync();

            return bookEntity.Id;
        }

        public async Task<Guid> UpdateAsync(Book book)
        {
            var bookEntity = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == book.Id);

            if(bookEntity is null)
            {
                return Guid.Empty;
            }

            bookEntity.ISBN = book.ISBN;
            bookEntity.Name = book.Name;
            bookEntity.Description = book.Description;
            bookEntity.Genre = book.Genre;
            bookEntity.TakenAt = book.TakenAt;
            bookEntity.ReturnBy = book.ReturnBy;
            bookEntity.ImagePath = book.ImagePath;

            await _context.SaveChangesAsync();

            return bookEntity.Id;
        }

        public async Task<Guid> IssueAsync(Guid id, DateTime returnBy)
        {

            var bookEntity = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id);

            if(bookEntity is null)
            {
                return Guid.Empty;
            }

            bookEntity.TakenAt = DateTime.Now;
            bookEntity.ReturnBy = returnBy;

            await _context.SaveChangesAsync();

            return bookEntity.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var bookEntity = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id);

            if(bookEntity is null)
            {
                return Guid.Empty;
            }

            _context.Books.Remove(bookEntity);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<Guid> DeleteAuthorAsync(Guid bookId, Guid authorId)
        {
            var bookEntity = await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if(bookEntity is null)
            {
                return Guid.Empty;
            }

            var authorEntity = bookEntity.Authors.FirstOrDefault(a => a.Id == authorId);

            if(authorEntity is null)
            {
                return Guid.Empty;
            }

            bookEntity.Authors.Remove(authorEntity);
            await _context.SaveChangesAsync();

            return bookEntity.Id;
        }

        public async Task<Book> GetAsync(Guid id)
        {
            var bookEntity = await _context.Books
                .Include(b => b.Authors)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);

            if(bookEntity is null)
            {
                return null;
            }

            return _mapper.Map<Book>(bookEntity);
        }

        public async Task<Book> GetByIsbnAsync(string isbn)
        {
            var bookEntity = await _context.Books
                .Include(b => b.Authors)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.ISBN == isbn);

            if(bookEntity is null)
            {
                return null;
            }

            return _mapper.Map<Book>(bookEntity);
        }

        public async Task<Book> GetByAuthorAsync(Guid authorId)
        {
            var bookEntity = await _context.Books
                .Include(b => b.Authors)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Authors.Any(a => a.Id == authorId));

            if(bookEntity is null)
            {
                return null;
            }

            return _mapper.Map<Book>(bookEntity);
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            var booksEntities = await _context.Books
                .Include(b => b.Authors)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<Book>>(booksEntities);
        }

        public async Task<IEnumerable<Book>> GetPageAsync(int pageIndex, int pageSize)
        {
            var booksEntities = await _context.Books
                .Include(b => b.Authors)
                .AsNoTracking()
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<List<Book>>(booksEntities);
        }
    }
}
