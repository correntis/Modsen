using Library.DataAccess;
using Library.Core.Entities;
using Library.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests.Unit
{
    public class BooksRepositoryTests
    {
        private readonly LibraryDbContext _context;
        private readonly BooksRepository _repository;

        public BooksRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "LibraryDb")
                .Options;

            _context = new LibraryDbContext(options);
            _repository = new BooksRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddBookToDatabase()
        {
            // Arrange
            var bookEntity = new BookEntity
            {
                Name = "Test Book",
                ISBN = "1234567890",
                Genre = "Test Genre",
                ImagePath = "default_image.jpg"
            };

            // Act
            await _repository.AddAsync(bookEntity);
            await _context.SaveChangesAsync();

            // Assert
            var addedBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookEntity.Id);
            Assert.NotNull(addedBook);
            Assert.Equal(bookEntity.Name, addedBook.Name);
            Assert.Equal(bookEntity.ISBN, addedBook.ISBN);
            Assert.Equal(bookEntity.Genre, addedBook.Genre);
            Assert.Equal(bookEntity.ImagePath, addedBook.ImagePath);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveBookFromDatabase()
        {
            // Arrange
            var bookEntity = new BookEntity
            {
                Name = "Test Book",
                ISBN = "1234567890",
                Genre = "Test Book",
                ImagePath = "default_image.jpg"
            };

            await _context.Books.AddAsync(bookEntity);
            await _context.SaveChangesAsync();

            // Act
            _repository.Delete(bookEntity);
            await _context.SaveChangesAsync();

            // Assert
            var deletedBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookEntity.Id);
            Assert.Null(deletedBook);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnBookById()
        {
            // Arrange
            var bookEntity = new BookEntity
            {
                Name = "Test Book",
                ISBN = "1234567890",
                Genre = "Test Book",
                ImagePath = "default_image.jpg"
            };

            await _context.Books.AddAsync(bookEntity);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAsync(bookEntity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookEntity.Id, result.Id);
            Assert.Equal(bookEntity.Name, result.Name);
            Assert.Equal(bookEntity.ISBN, result.ISBN);
            Assert.Equal(bookEntity.Genre, result.Genre);
        }
    }
}
