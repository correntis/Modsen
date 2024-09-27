using AutoMapper;
using FluentValidation;
using Library.API.Contracts;
using Library.API.Validation;
using Library.Core.Abstractions;
using Library.Core.Exceptions;
using Library.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/v1/books")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBooksService _booksService;
        private readonly IMapper _mapper;
        private readonly IValidator<BookContract> _booksValidator;

        public BooksController(
            ILogger<BooksController> logger,
            IBooksService booksService,
            IMapper mapper,
            IValidator<BookContract> booksValidator
            )
        {
            _logger = logger;
            _booksService = booksService;
            _mapper = mapper;
            _booksValidator = booksValidator;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(BookContract bookContract)
        {
            await _booksValidator.ValidateAndThrowAsync(bookContract);

            var book = _mapper.Map<Book>(bookContract);
            
            return Ok(await _booksService.AddAsync(book));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{bookId}/authors/{authorId}")]
        public async Task<IActionResult> AddAuthor(Guid bookId, Guid authorId)
        {
            await _booksService.AddAuthorAsync(bookId, authorId);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, BookContract bookContract)
        {
            await _booksValidator.ValidateAndThrowAsync(bookContract);

            var book = _mapper.Map<Book>(bookContract);
            book.Id = id;
            
            await _booksService.UpdateAsync(book);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _booksService.DeleteAsync(id);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{bookId}/authors/{authorId}")]
        public async Task<IActionResult> DeleteAuthor(Guid bookId, Guid authorId)
        {
            await _booksService.DeleteAuthorAsync(bookId, authorId);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _booksService.GetAsync(id));
        }

        [HttpGet("authors/{authorId}")]
        public async Task<IActionResult> GetByAuthor(Guid authorId)
        {
            return Ok(await _booksService.GetByAuthorAsync(authorId));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _booksService.GetAllAsync());
        }

        [HttpGet("pages")]
        public async Task<IActionResult> GetPage(int pageIndex, int pageSize, [FromQuery] BooksFilter filter)
        {
            return Ok(await _booksService.GetPageAsync(pageIndex, pageSize, filter));
        }

        [HttpGet("amount")]
        public async Task<IActionResult> GetBooksAmount([FromQuery] BooksFilter filter)
        {
            return Ok(await _booksService.GetAmountAsync(filter));
        }
    }
}
