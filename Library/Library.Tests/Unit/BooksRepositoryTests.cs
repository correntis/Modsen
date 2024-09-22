using AutoMapper;
using Library.Core.Models;
using Library.DataAccess;
using Library.DataAccess.Entities;
using Library.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Library.Tests.Unit
{
    public class BooksRepositoryTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddBookAndReturnBookId()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            var logger = Mock.Of<ILogger<BooksRepository>>();

            var booksRepository = new BooksRepository(context, logger, mapper);

            var newBook = new Book
            {
                Name = "Test Book",
                ISBN = "1234567890",
                Description = "Test Description",
                Genre = "Test Genre",
                ImagePath = "default_image.jpg"
            };

            // Act
            var result = await booksRepository.AddAsync(newBook);

            // Assert
            var addedBook = await context.Books.FindAsync(result);
            Assert.NotNull(addedBook);
            Assert.Equal(newBook.Name, addedBook.Name);
            Assert.Equal(newBook.ISBN, addedBook.ISBN);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            var logger = Mock.Of<ILogger<BooksRepository>>();

            var booksRepository = new BooksRepository(context, logger, mapper);

            var bookEntity = new BookEntity
            {
                Name = "Test Book",
                ISBN = "1234567890",
                Description = "Test Description",
                Genre = "Test Genre",
                ImagePath = "default_image.jpg"
            };

            await context.Books.AddAsync(bookEntity);
            await context.SaveChangesAsync();

            // Act
            var result = await booksRepository.GetAsync(bookEntity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookEntity.Name, result.Name);
            Assert.Equal(bookEntity.ISBN, result.ISBN);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenBookDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            var logger = Mock.Of<ILogger<BooksRepository>>();

            var booksRepository = new BooksRepository(context, logger, mapper);

            var nonExistingBookId = Guid.NewGuid();

            // Act
            var result = await booksRepository.GetAsync(nonExistingBookId);

            // Assert
            Assert.Null(result);
        }

        private LibraryDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new LibraryDbContext(options);
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Book, BookEntity>().ReverseMap();
            });

            return config.CreateMapper();
        }
    }
}
