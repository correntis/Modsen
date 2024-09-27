using Moq;
using AutoMapper;
using Library.Application.Services;
using Library.Core.Abstractions;
using Library.Core.Entities;
using Library.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Library.Tests.Unit
{
    public class BooksServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly BooksService _booksService;

        public BooksServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _fileServiceMock = new Mock<IFileService>();

            _booksService = new BooksService(_unitOfWorkMock.Object, _mapperMock.Object, _fileServiceMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldAddBookAndReturnId()
        {
            // Arrange
            var book = new Book { Name = "Test Book", ImageFile = new Mock<IFormFile>().Object };
            var bookEntity = new BookEntity { Id = Guid.NewGuid() };

            _fileServiceMock.Setup(fs => fs.SaveAsync(book.ImageFile)).ReturnsAsync("test/path");
            _mapperMock.Setup(m => m.Map<BookEntity>(book)).Returns(bookEntity);
            _unitOfWorkMock.Setup(u => u.BooksRepository.AddAsync(It.IsAny<BookEntity>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _booksService.AddAsync(book);

            // Assert
            Assert.Equal(bookEntity.Id, result);
            _fileServiceMock.Verify(fs => fs.SaveAsync(book.ImageFile), Times.Once);
            _unitOfWorkMock.Verify(u => u.BooksRepository.AddAsync(bookEntity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAuthorAsync_ShouldAddAuthorToBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var authorId = Guid.NewGuid();
            var bookEntity = new BookEntity { Id = bookId, Authors = new List<AuthorEntity>() };
            var authorEntity = new AuthorEntity { Id = authorId };

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetAsync(bookId)).ReturnsAsync(bookEntity);
            _unitOfWorkMock.Setup(u => u.AuthorsRepository.GetAsync(authorId)).ReturnsAsync(authorEntity);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _booksService.AddAuthorAsync(bookId, authorId);

            // Assert
            Assert.Contains(authorEntity, bookEntity.Authors);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateBookDetails()
        {
            // Arrange
            var book = new Book { Id = Guid.NewGuid(), Name = "Updated Book", ISBN = "123456", Genre = "Fiction" };
            var bookEntity = new BookEntity { Id = book.Id };

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetAsync(book.Id)).ReturnsAsync(bookEntity);
            _fileServiceMock.Setup(fs => fs.SaveAsync(book.ImageFile)).ReturnsAsync("updated/path");
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _booksService.UpdateAsync(book);

            // Assert
            Assert.Equal(book.Name, bookEntity.Name);
            Assert.Equal(book.ISBN, bookEntity.ISBN);
            Assert.Equal(book.Genre, bookEntity.Genre);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveBookFromRepository()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var bookEntity = new BookEntity { Id = bookId };

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetAsync(bookId)).ReturnsAsync(bookEntity);
            _unitOfWorkMock.Setup(u => u.BooksRepository.Delete(bookEntity));
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _booksService.DeleteAsync(bookId);

            // Assert
            _unitOfWorkMock.Verify(u => u.BooksRepository.Delete(bookEntity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAuthorAsync_ShouldRemoveAuthorFromBook()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var authorId = Guid.NewGuid();
            var authorEntity = new AuthorEntity { Id = authorId };
            var bookEntity = new BookEntity { Id = bookId, Authors = new List<AuthorEntity> { authorEntity } };

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetAsync(bookId)).ReturnsAsync(bookEntity);
            _unitOfWorkMock.Setup(u => u.AuthorsRepository.GetAsync(authorId)).ReturnsAsync(authorEntity);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _booksService.DeleteAuthorAsync(bookId, authorId);

            // Assert
            Assert.DoesNotContain(authorEntity, bookEntity.Authors);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnBookById()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var bookEntity = new BookEntity { Id = bookId };

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetAsync(bookId)).ReturnsAsync(bookEntity);

            // Act
            var result = await _booksService.GetAsync(bookId);

            // Assert
            Assert.Equal(bookEntity, result);
        }

        [Fact]
        public async Task GetByIsbnAsync_ShouldReturnBookByIsbn()
        {
            // Arrange
            var isbn = "123456";
            var bookEntity = new BookEntity { ISBN = isbn };

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetByIsbnAsync(isbn)).ReturnsAsync(bookEntity);

            // Act
            var result = await _booksService.GetByIsbnAsync(isbn);

            // Assert
            Assert.Equal(bookEntity, result);
        }

        [Fact]
        public async Task GetByAuthorAsync_ShouldReturnBookByAuthorId()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var bookEntity = new BookEntity { Authors = new List<AuthorEntity> { new AuthorEntity { Id = authorId } } };

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetByAuthorAsync(authorId)).ReturnsAsync(bookEntity);

            // Act
            var result = await _booksService.GetByAuthorAsync(authorId);

            // Assert
            Assert.Equal(bookEntity, result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBooks()
        {
            // Arrange
            var books = new List<BookEntity> { new BookEntity { Id = Guid.NewGuid() }, new BookEntity { Id = Guid.NewGuid() } };

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetAllAsync()).ReturnsAsync(books);

            // Act
            var result = await _booksService.GetAllAsync();

            // Assert
            Assert.Equal(books, result);
        }

        [Fact]
        public async Task GetPageAsync_ShouldReturnPagedBooks()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 10;
            var filter = new BooksFilter();
            var books = new List<BookEntity> { new BookEntity { Id = Guid.NewGuid() } };

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetPageAsync(pageIndex, pageSize, filter)).ReturnsAsync(books);

            // Act
            var result = await _booksService.GetPageAsync(pageIndex, pageSize, filter);

            // Assert
            Assert.Equal(books, result);
        }

        [Fact]
        public async Task GetAmountAsync_ShouldReturnBooksCount()
        {
            // Arrange
            var filter = new BooksFilter();
            var count = 42;

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetAmountAsync(filter)).ReturnsAsync(count);

            // Act
            var result = await _booksService.GetAmountAsync(filter);

            // Assert
            Assert.Equal(count, result);
        }
    }

}