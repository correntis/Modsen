using Moq;
using AutoMapper;
using Library.Application.Services;
using Library.Core.Abstractions;
using Library.Core.Entities;
using Library.Core.Models;
using Microsoft.AspNetCore.Http;
using Library.Application.UseCases.Books;

namespace Library.Tests.Unit
{
    public class BooksUseCasesTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFileService> _fileServiceMock;

        private readonly AddBookUseCase _addBookUseCase;
        private readonly AddBookAuthorUseCase _addBookAuthorUseCase;
        private readonly UpdateBookUseCase _updateBookUseCase;
        private readonly DeleteBookUseCase _deleteBookUseCase;
        private readonly DeleteBookAuthorUseCase _deleteBookAuthorUseCase;
        private readonly GetBookUseCase _getBookUseCase;
        private readonly GetBookByAuthorUseCase _getBookByAuthorUseCase;
        private readonly GetBookByIsbnUseCase _getBookByIsbnUseCase;
        private readonly GetAllBooksUseCase _getAllBooksUseCase;
        private readonly GetBooksPageUseCase _getBooksPageUseCase;
        private readonly GetBooksAmountUseCase _getBooksAmountUseCase;

        public BooksUseCasesTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _fileServiceMock = new Mock<IFileService>();

            _addBookUseCase = new AddBookUseCase(_unitOfWorkMock.Object, _mapperMock.Object, _fileServiceMock.Object);
            _addBookAuthorUseCase= new AddBookAuthorUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
            _updateBookUseCase = new UpdateBookUseCase(_unitOfWorkMock.Object, _mapperMock.Object, _fileServiceMock.Object);
            _deleteBookUseCase = new DeleteBookUseCase(_unitOfWorkMock.Object);
            _deleteBookAuthorUseCase = new DeleteBookAuthorUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
            _getBookUseCase = new GetBookUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
            _getBookByAuthorUseCase = new GetBookByAuthorUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
            _getBookByIsbnUseCase = new GetBookByIsbnUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
            _getAllBooksUseCase = new GetAllBooksUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
            _getBooksPageUseCase = new GetBooksPageUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
            _getBooksAmountUseCase = new GetBooksAmountUseCase(_unitOfWorkMock.Object);

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
            var result = await _addBookUseCase.ExecuteAsync(book);

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
            await _addBookAuthorUseCase.ExecuteAsync(bookId, authorId);

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
            await _updateBookUseCase.ExecuteAsync(book);

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
            await _deleteBookUseCase.ExecuteAsync(bookId);

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
            await _deleteBookAuthorUseCase.ExecuteAsync(bookId, authorId);

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
            var bookModel = new Book { Id = bookId };

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetAsync(bookId)).ReturnsAsync(bookEntity);
            _mapperMock.Setup(m => m.Map<Book>(bookEntity)).Returns(bookModel);

            // Act
            var result = await _getBookUseCase.ExecuteAsync(bookId);

            // Assert
            Assert.Equal(bookModel, result);
        }


        [Fact]
        public async Task GetByIsbnAsync_ShouldReturnBookByIsbn()
        {
            // Arrange
            var isbn = "123456";
            var bookEntity = new BookEntity { ISBN = isbn };
            var bookModel = new Book { ISBN = isbn };

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetByIsbnAsync(isbn)).ReturnsAsync(bookEntity);
            _mapperMock.Setup(m => m.Map<Book>(bookEntity)).Returns(bookModel);

            // Act
            var result = await _getBookByIsbnUseCase.ExecuteAsync(isbn);

            // Assert
            Assert.Equal(bookModel, result);
        }


        [Fact]
        public async Task GetByAuthorAsync_ShouldReturnBookByAuthorId()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var bookEntity = new BookEntity { Authors = new List<AuthorEntity> { new AuthorEntity { Id = authorId } } };
            var bookModel = new Book { Authors = new List<Author> { new Author { Id = authorId } } };

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetByAuthorAsync(authorId)).ReturnsAsync(bookEntity);
            _mapperMock.Setup(m => m.Map<Book>(bookEntity)).Returns(bookModel);

            // Act
            var result = await _getBookByAuthorUseCase.ExecuteAsync(authorId);

            // Assert
            Assert.Equal(bookModel, result);
        }


        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBooks()
        {
            // Arrange
            var booksEntities = new List<BookEntity>
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            var booksModels = new List<Book>
            {
                new() { Id = booksEntities[0].Id },
                new() { Id = booksEntities[1].Id }
            };

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetAllAsync()).ReturnsAsync(booksEntities);
            _mapperMock.Setup(m => m.Map<List<Book>>(booksEntities)).Returns(booksModels);

            // Act
            var result = await _getAllBooksUseCase.ExecuteAsync();

            // Assert
            Assert.Equal(booksModels, result);
        }


        [Fact]
        public async Task GetPageAsync_ShouldReturnPagedBooks()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 10;
            var filter = new BooksFilter();
            var booksEntities = new List<BookEntity>
            {
                new() { Id = Guid.NewGuid() }
            };
            var booksModels = new List<Book>
            {
                new() { Id = booksEntities[0].Id }
            };

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetPageAsync(pageIndex, pageSize, filter)).ReturnsAsync(booksEntities);
            _mapperMock.Setup(m => m.Map<List<Book>>(booksEntities)).Returns(booksModels);

            // Act
            var result = await _getBooksPageUseCase.ExecuteAsync(pageIndex, pageSize, filter);

            // Assert
            Assert.Equal(booksModels, result);
        }


        [Fact]
        public async Task GetAmountAsync_ShouldReturnBooksCount()
        {
            // Arrange
            var filter = new BooksFilter();
            var count = 42;

            _unitOfWorkMock.Setup(u => u.BooksRepository.GetAmountAsync(filter)).ReturnsAsync(count);

            // Act
            var result = await _getBooksAmountUseCase.ExecuteAsync(filter);

            // Assert
            Assert.Equal(count, result);
        }
    }

}