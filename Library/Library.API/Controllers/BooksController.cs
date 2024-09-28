using AutoMapper;
using FluentValidation;
using Library.API.Contracts;
using Library.Application.UseCases.Books;
using Library.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/v1/books")]
    public class BooksController : ControllerBase
    {
        private readonly IValidator<BookContract> _booksValidator;
        private readonly IMapper _mapper;

        private readonly AddBookUseCase _addBookUseCase;
        private readonly AddBookAuthorUseCase _addBookAuthorUseCase;
        private readonly UpdateBookUseCase _updateBookUseCase;
        private readonly DeleteBookUseCase _deleteBookUseCase;
        private readonly DeleteBookAuthorUseCase _deleteBookAuthorUseCase;
        private readonly GetBookUseCase _getBookUseCase;
        private readonly GetBookByAuthorUseCase _getBookByAuthorUseCase;
        private readonly GetAllBooksUseCase _getAllBooksUseCase;
        private readonly GetBooksPageUseCase _getBooksPageUseCase;
        private readonly GetBooksAmountUseCase _getBooksAmountUseCase;

        public BooksController(
            IValidator<BookContract> booksValidator,
            IMapper mapper,
            AddBookUseCase addBookUseCase,
            AddBookAuthorUseCase addBookAuthorUseCase,
            UpdateBookUseCase updateBookUseCase,
            DeleteBookUseCase deleteBookUseCase,
            DeleteBookAuthorUseCase deleteBookAuthorUseCase,
            GetBookUseCase getBookUseCase,
            GetBookByAuthorUseCase getBookByAuthorUseCase,
            GetAllBooksUseCase getAllBooksUseCase,
            GetBooksPageUseCase getBooksPageUseCase,
            GetBooksAmountUseCase getBooksAmountUseCase
            )
        {
            _booksValidator = booksValidator;
            _mapper = mapper;
            _addBookUseCase = addBookUseCase;
            _addBookAuthorUseCase = addBookAuthorUseCase;
            _updateBookUseCase = updateBookUseCase;
            _deleteBookUseCase = deleteBookUseCase;
            _deleteBookAuthorUseCase = deleteBookAuthorUseCase;
            _getBookUseCase = getBookUseCase;
            _getBookByAuthorUseCase = getBookByAuthorUseCase;
            _getAllBooksUseCase = getAllBooksUseCase;
            _getBooksPageUseCase = getBooksPageUseCase;
            _getBooksAmountUseCase = getBooksAmountUseCase;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(BookContract bookContract)
        {
            await _booksValidator.ValidateAndThrowAsync(bookContract);

            var book = _mapper.Map<Book>(bookContract);
            
            return Ok(await _addBookUseCase.ExecuteAsync(book));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{bookId}/authors/{authorId}")]
        public async Task<IActionResult> AddAuthor(Guid bookId, Guid authorId)
        {
            await _addBookAuthorUseCase.ExecuteAsync(bookId, authorId);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, BookContract bookContract)
        {
            await _booksValidator.ValidateAndThrowAsync(bookContract);

            var book = _mapper.Map<Book>(bookContract);
            book.Id = id;
            
            await _updateBookUseCase.ExecuteAsync(book);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _deleteBookUseCase.ExecuteAsync(id);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{bookId}/authors/{authorId}")]
        public async Task<IActionResult> DeleteAuthor(Guid bookId, Guid authorId)
        {
            await _deleteBookAuthorUseCase.ExecuteAsync(bookId, authorId);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _getBookUseCase.ExecuteAsync(id));
        }

        [HttpGet("authors/{authorId}")]
        public async Task<IActionResult> GetByAuthor(Guid authorId)
        {
            return Ok(await _getBookByAuthorUseCase.ExecuteAsync(authorId));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _getAllBooksUseCase.ExecuteAsync());
        }

        [HttpGet("pages")]
        public async Task<IActionResult> GetPage(int pageIndex, int pageSize, [FromQuery] BooksFilter filter)
        {
            return Ok(await _getBooksPageUseCase.ExecuteAsync(pageIndex, pageSize, filter));
        }

        [HttpGet("amount")]
        public async Task<IActionResult> GetBooksAmount([FromQuery] BooksFilter filter)
        {
            return Ok(await _getBooksAmountUseCase.ExecuteAsync(filter));
        }
    }
}
