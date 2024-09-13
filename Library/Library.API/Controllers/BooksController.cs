using AutoMapper;
using Library.API.Contracts;
using Library.Core.Abstractions;
using Library.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/v1/books")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBooksService _booksService;
        private readonly IMapper _mapper;

        public BooksController(
            ILogger<BooksController> logger,
            IBooksService booksService,
            IMapper mapper
            )
        {
            _logger = logger;
            _booksService = booksService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Add(BookContract bookContract)
        {
            var book = _mapper.Map<Book>(bookContract);

            var guid = await _booksService.AddAsync(book);
            
            return Ok(guid);
        }

        [HttpPut("{bookId}/authors/{authorId}")]
        public async Task<IActionResult> AddAuthor(Guid bookId, Guid authorId)
        {
            var guid = await _booksService.AddAuthorAsync(bookId, authorId);

            return Ok(guid);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, BookContract bookContract)
        {
            var book = _mapper.Map<Book>(bookContract);
            book.Id = id;

            var guid = await _booksService.UpdateAsync(book);

            return Ok(guid);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var guid = await _booksService.DeleteAsync(id);

            return Ok(guid);
        }

        [HttpDelete("{bookId}/authors/{authorId}")]
        public async Task<IActionResult> DeleteAuthor(Guid bookId, Guid authorId)
        {
            var guid = await _booksService.DeleteAuthorAsync(bookId, authorId);

            return Ok(guid);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var book = await _booksService.GetAsync(id);

            return Ok(book);
        }

        [HttpGet("authors/{authorId}")]
        public async Task<IActionResult> GetByAuthor(Guid authorId)
        {
            var book = await _booksService.GetByAuthorAsync(authorId);

            return Ok(book);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _booksService.GetAllAsync();

            return Ok(books);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetPage(int pageIndex, int pageSize)
        {
            var books = await _booksService.GetPageAsync(pageIndex, pageSize);

            return Ok(books);
        }
    }
}
