using Library.Core.Abstractions;
using Library.Core.Models;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.DataAccess.Repositories
{
    public class AuthorsRepository : IAuthorsRepository
    {
        private readonly LibraryDbContext _context;
        private readonly ILogger<AuthorsRepository> _logger;

        public AuthorsRepository(
            LibraryDbContext context,
            ILogger<AuthorsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> Create(Author author)
        {
            var authorEntity = new AuthorEntity
            {
                Id = author.Id,
                Name = author.Name,
                Surname = author.Surname,
                Birthday = author.Birthday,
                Country = author.Country
            };

            _context.Authors.Add(authorEntity);
            await _context.SaveChangesAsync();

            return authorEntity.Id;
        }

        public async Task<Guid> Update(Author author)
        {
            var authorEntity = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == author.Id);

            if(authorEntity is null)
            {
                return Guid.Empty;
            }

            authorEntity.Name = author.Name;
            authorEntity.Surname = author.Surname;
            authorEntity.Birthday = author.Birthday;
            authorEntity.Country = author.Country;

            authorEntity.Books = author.Books.Select(bookEntity => new BookEntity
            {
                Id = bookEntity.Id,
                ISBN = bookEntity.ISBN,
                Name = bookEntity.Name,
                Description = bookEntity.Description,
                Genre = bookEntity.Genre,
                TakenAt = bookEntity.TakenAt,
                ReturnBy = bookEntity.ReturnBy,
                ImagePath = bookEntity.ImagePath
            }).ToList();

            await _context.SaveChangesAsync();

            return authorEntity.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            var authorEntity = await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == id);

            if(authorEntity is null)
            {
                return Guid.Empty;
            }

            _context.Authors.Remove(authorEntity);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<Author> Get(Guid id)
        {
            var authorEntity = await _context.Authors
                .Include(a => a.Books)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if(authorEntity is null)
            {
                return null;
            }

            return new Author
            {
                Id = authorEntity.Id,
                Name = authorEntity.Name,
                Surname = authorEntity.Surname,
                Birthday = authorEntity.Birthday,
                Country = authorEntity.Country,
                Books = authorEntity.Books.Select(bookEntity => new Book
                {
                    Id = bookEntity.Id,
                    ISBN = bookEntity.ISBN,
                    Name = bookEntity.Name,
                    Description = bookEntity.Description,
                    Genre = bookEntity.Genre,
                    TakenAt = bookEntity.TakenAt,
                    ReturnBy = bookEntity.ReturnBy,
                    ImagePath = bookEntity.ImagePath
                }).ToList()
            };
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            var authorEntities = await _context.Authors
                .Include(a => a.Books)
                .AsNoTracking()
                .ToListAsync();

            return authorEntities.Select(authorEntity => new Author
            {
                Id = authorEntity.Id,
                Name = authorEntity.Name,
                Surname = authorEntity.Surname,
                Birthday = authorEntity.Birthday,
                Country = authorEntity.Country,
                Books = authorEntity.Books.Select(bookEntity => new Book
                {
                    Id = bookEntity.Id,
                    ISBN = bookEntity.ISBN,
                    Name = bookEntity.Name,
                    Description = bookEntity.Description,
                    Genre = bookEntity.Genre,
                    TakenAt = bookEntity.TakenAt,
                    ReturnBy = bookEntity.ReturnBy,
                    ImagePath = bookEntity.ImagePath
                }).ToList()
            }).ToList();
        }

        public async Task<IEnumerable<Author>> GetPage(int pageIndex, int pageSize)
        {
            var authorEntities = await _context.Authors
                .Include(a => a.Books)
                .AsNoTracking()
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return authorEntities.Select(authorEntity => new Author
            {
                Id = authorEntity.Id,
                Name = authorEntity.Name,
                Surname = authorEntity.Surname,
                Birthday = authorEntity.Birthday,
                Country = authorEntity.Country,
                Books = authorEntity.Books.Select(bookEntity => new Book
                {
                    Id = bookEntity.Id,
                    ISBN = bookEntity.ISBN,
                    Name = bookEntity.Name,
                    Description = bookEntity.Description,
                    Genre = bookEntity.Genre,
                    TakenAt = bookEntity.TakenAt,
                    ReturnBy = bookEntity.ReturnBy,
                    ImagePath = bookEntity.ImagePath
                }).ToList()
            }).ToList();
        }
    }
}
