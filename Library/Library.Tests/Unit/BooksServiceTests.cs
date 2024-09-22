using AutoMapper;
using Library.Application.Services;
using Library.Core.Abstractions;
using Library.Core.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests.Unit
{
    public class BooksServiceTests
    {
        private readonly Mock<IBooksRepository> _booksRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BooksService _booksService;

        public BooksServiceTests()
        {
            _booksRepositoryMock = new Mock<IBooksRepository>();
            _mapperMock = new Mock<IMapper>();
            _booksService = new BooksService(_booksRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddAsync_Should_SetDefaultValues_AndCallRepositoryAdd()
        {
            // Arrange
            var book = new Book();
            var bookId = Guid.NewGuid();

            _booksRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Book>())).ReturnsAsync(bookId);

            // Act
            var result = await _booksService.AddAsync(book);

            // Assert
            Assert.Equal(DateTime.MinValue, book.TakenAt);
            Assert.Equal(DateTime.MinValue, book.ReturnBy);
            _booksRepositoryMock.Verify(r => r.AddAsync(book), Times.Once);
            Assert.Equal(bookId, result);
        }

        [Fact]
        public async Task AddAuthorAsync_Should_CallRepositoryAddAuthor_WithCorrectIds()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var authorId = Guid.NewGuid();

            _booksRepositoryMock.Setup(r => r.AddAuthorAsync(bookId, authorId)).ReturnsAsync(bookId);

            // Act
            var result = await _booksService.AddAuthorAsync(bookId, authorId);

            // Assert
            _booksRepositoryMock.Verify(r => r.AddAuthorAsync(bookId, authorId), Times.Once);
            Assert.Equal(bookId, result);
        }


        [Fact]
        public async Task UpdateAsync_Should_CallRepositoryUpdate_WithCorrectBook()
        {
            // Arrange
            var book = new Book { Id = Guid.NewGuid() };

            _booksRepositoryMock.Setup(r => r.UpdateAsync(book)).ReturnsAsync(book.Id);

            // Act
            var result = await _booksService.UpdateAsync(book);

            // Assert
            _booksRepositoryMock.Verify(r => r.UpdateAsync(book), Times.Once);
            Assert.Equal(book.Id, result);
        }


        [Fact]
        public async Task DeleteAsync_Should_CallRepositoryDelete_WithCorrectId()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            _booksRepositoryMock.Setup(r => r.DeleteAsync(bookId)).ReturnsAsync(bookId);

            // Act
            var result = await _booksService.DeleteAsync(bookId);

            // Assert
            _booksRepositoryMock.Verify(r => r.DeleteAsync(bookId), Times.Once);
            Assert.Equal(bookId, result);
        }

        [Fact]
        public async Task DeleteAuthorAsync_Should_CallRepositoryDeleteAuthor_WithCorrectIds()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var authorId = Guid.NewGuid();

            _booksRepositoryMock.Setup(r => r.DeleteAuthorAsync(bookId, authorId)).ReturnsAsync(bookId);

            // Act
            var result = await _booksService.DeleteAuthorAsync(bookId, authorId);

            // Assert
            _booksRepositoryMock.Verify(r => r.DeleteAuthorAsync(bookId, authorId), Times.Once);
            Assert.Equal(bookId, result);
        }


        [Fact]
        public async Task GetAsync_Should_CallRepositoryGet_WithCorrectId()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new Book { Id = bookId };

            _booksRepositoryMock.Setup(r => r.GetAsync(bookId)).ReturnsAsync(book);

            // Act
            var result = await _booksService.GetAsync(bookId);

            // Assert
            _booksRepositoryMock.Verify(r => r.GetAsync(bookId), Times.Once);
            Assert.Equal(book, result);
        }

        [Fact]
        public async Task GetByAuthorAsync_Should_CallRepositoryGetByAuthor_WithCorrectAuthorId()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var book = new Book();

            _booksRepositoryMock.Setup(r => r.GetByAuthorAsync(authorId)).ReturnsAsync(book);

            // Act
            var result = await _booksService.GetByAuthorAsync(authorId);

            // Assert
            _booksRepositoryMock.Verify(r => r.GetByAuthorAsync(authorId), Times.Once);
            Assert.Equal(book, result);
        }

        [Fact]
        public async Task GetByIsbnAsync_Should_CallRepositoryGetByIsbn_WithCorrectIsbn()
        {
            // Arrange
            var isbn = "123456789";
            var book = new Book();

            _booksRepositoryMock.Setup(r => r.GetByIsbnAsync(isbn)).ReturnsAsync(book);

            // Act
            var result = await _booksService.GetByIsbnAsync(isbn);

            // Assert
            _booksRepositoryMock.Verify(r => r.GetByIsbnAsync(isbn), Times.Once);
            Assert.Equal(book, result);
        }

        [Fact]
        public async Task GetAllAsync_Should_CallRepositoryGetAll()
        {
            // Arrange
            var books = new List<Book> { new Book(), new Book() };

            _booksRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(books);

            // Act
            var result = await _booksService.GetAllAsync();

            // Assert
            _booksRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
            Assert.Equal(books, result);
        }

        [Fact]
        public async Task GetPageAsync_Should_CallRepositoryGetPage_WithCorrectParams()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 10;
            var filter = new BooksFilter();
            var books = new List<Book>();

            _booksRepositoryMock.Setup(r => r.GetPageAsync(pageIndex, pageSize, filter)).ReturnsAsync(books);

            // Act
            var result = await _booksService.GetPageAsync(pageIndex, pageSize, filter);

            // Assert
            _booksRepositoryMock.Verify(r => r.GetPageAsync(pageIndex, pageSize, filter), Times.Once);
            Assert.Equal(books, result);
        }

        [Fact]
        public async Task GetAmountAsync_Should_CallRepositoryGetAmount_WithCorrectFilter()
        {
            // Arrange
            var filter = new BooksFilter();
            var amount = 100;

            _booksRepositoryMock.Setup(r => r.GetAmountAsync(filter)).ReturnsAsync(amount);

            // Act
            var result = await _booksService.GetAmountAsync(filter);

            // Assert
            _booksRepositoryMock.Verify(r => r.GetAmountAsync(filter), Times.Once);
            Assert.Equal(amount, result);
        }
    }
}
