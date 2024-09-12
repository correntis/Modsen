using Library.Core.Abstraction;
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

        public BooksRepository(
            LibraryDbContext context,
            ILogger<BooksRepository> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> Create(Book book)
        {
            var bookEntity = new BookEntity
            {
                Id = book.Id,
                ISBN = book.ISBN,
                Name = book.Name,
                Description = book.Description,
                Genre = book.Genre,
                TakenAt = book.TakenAt,
                ReturnBy = book.ReturnBy,
                ImagePath = book.ImagePath,
                Authors = book.Authors.Select(authorEntity => new AuthorEntity
                {
                    Id = authorEntity.Id,
                    Name = authorEntity.Name,
                    Surname = authorEntity.Surname,
                    Birthday = authorEntity.Birthday,
                    Country = authorEntity.Country
                }).ToList()
            };

            _context.Books.Add(bookEntity);
            await _context.SaveChangesAsync();

            return bookEntity.Id;
        }

        public async Task<Guid> Update(Book book)
        {
            var bookEntity = await _context.Books
                .Include(b => b.Authors)
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

            bookEntity.Authors = book.Authors.Select(authorEntity => new AuthorEntity
            {
                Id = authorEntity.Id,
                Name = authorEntity.Name,
                Surname = authorEntity.Surname,
                Birthday = authorEntity.Birthday,
                Country = authorEntity.Country
            }).ToList();

            await _context.SaveChangesAsync();

            return bookEntity.Id;
        }

        public async Task<Guid> Issue(Guid id, DateTime returnBy)
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

        public async Task<Guid> Delete(Guid id)
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

        public async Task<Book> Get(Guid id)
        {
            var bookEntity = await _context.Books
                .Include(b => b.Authors)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);

            if(bookEntity is null)
            {
                return null;
            }

            return new Book
            {
                Id = bookEntity.Id,
                ISBN = bookEntity.ISBN,
                Name = bookEntity.Name,
                Description = bookEntity.Description,
                Genre = bookEntity.Genre,
                TakenAt = bookEntity.TakenAt,
                ReturnBy = bookEntity.ReturnBy,
                ImagePath = bookEntity.ImagePath,
                Authors = bookEntity.Authors.Select(authorEntity => new Author
                {
                    Id = authorEntity.Id,
                    Name = authorEntity.Name,
                    Surname = authorEntity.Surname,
                    Birthday = authorEntity.Birthday,
                    Country = authorEntity.Country
                }).ToList()
            };
        }

        public async Task<Book> GetByISBN(string isbn)
        {
            var bookEntity = await _context.Books
                .Include(b => b.Authors)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.ISBN == isbn);

            if(bookEntity is null)
            {
                return null;
            }

            return new Book
            {
                Id = bookEntity.Id,
                ISBN = bookEntity.ISBN,
                Name = bookEntity.Name,
                Description = bookEntity.Description,
                Genre = bookEntity.Genre,
                TakenAt = bookEntity.TakenAt,
                ReturnBy = bookEntity.ReturnBy,
                ImagePath = bookEntity.ImagePath,
                Authors = bookEntity.Authors.Select(authorEntity => new Author
                {
                    Id = authorEntity.Id,
                    Name = authorEntity.Name,
                    Surname = authorEntity.Surname,
                    Birthday = authorEntity.Birthday,
                    Country = authorEntity.Country
                }).ToList()
            };
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            var booksEntities = await _context.Books
                .Include(b => b.Authors)
                .AsNoTracking()
                .ToListAsync();

            return booksEntities.Select(bookEntity => new Book
            {
                Id = bookEntity.Id,
                ISBN = bookEntity.ISBN,
                Name = bookEntity.Name,
                Description = bookEntity.Description,
                Genre = bookEntity.Genre,
                TakenAt = bookEntity.TakenAt,
                ReturnBy = bookEntity.ReturnBy,
                ImagePath = bookEntity.ImagePath,
                Authors = bookEntity.Authors.Select(authorEntity => new Author
                {
                    Id = authorEntity.Id,
                    Name = authorEntity.Name,
                    Surname = authorEntity.Surname,
                    Birthday = authorEntity.Birthday,
                    Country = authorEntity.Country
                }).ToList()
            });
        }

        public async Task<IEnumerable<Book>> GetPage(int pageIndex, int pageSize)
        {
            var booksEntities = await _context.Books
                .Include(b => b.Authors)
                .AsNoTracking()
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return booksEntities.Select(bookEntity => new Book
            {
                Id = bookEntity.Id,
                ISBN = bookEntity.ISBN,
                Name = bookEntity.Name,
                Description = bookEntity.Description,
                Genre = bookEntity.Genre,
                TakenAt = bookEntity.TakenAt,
                ReturnBy = bookEntity.ReturnBy,
                ImagePath = bookEntity.ImagePath,
                Authors = bookEntity.Authors.Select(authorEntity => new Author
                {
                    Id = authorEntity.Id,
                    Name = authorEntity.Name,
                    Surname = authorEntity.Surname,
                    Birthday = authorEntity.Birthday,
                    Country = authorEntity.Country
                }).ToList()
            });
        }
    }
}
