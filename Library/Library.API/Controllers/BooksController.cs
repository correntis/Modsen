using AutoMapper;
using Library.API.Contracts;
using Library.Core.Abstractions;
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
        private readonly IFileService _fileService;

        public BooksController(
            ILogger<BooksController> logger,
            IBooksService booksService,
            IMapper mapper,
            IFileService fileService
            )
        {
            _logger = logger;
            _booksService = booksService;
            _mapper = mapper;
            _fileService = fileService;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(BookContract bookContract)
        {
            var book = _mapper.Map<Book>(bookContract);
            book.ImagePath = _fileService.DefaultImagePath;

            if(bookContract.ImageFile != null)
            {
                book.ImagePath = await _fileService.SaveAsync(bookContract.ImageFile);
            }

            var guid = await _booksService.AddAsync(book);
            
            return Ok(guid);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{bookId}/authors/{authorId}")]
        public async Task<IActionResult> AddAuthor(Guid bookId, Guid authorId)
        {
            var guid = await _booksService.AddAuthorAsync(bookId, authorId);

            return Ok(guid);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, BookContract bookContract)
        {
            var existingBook = await _booksService.GetAsync(id);
            if(existingBook == null)
            {
                return NotFound();
            }

            var book = _mapper.Map<Book>(bookContract);
            book.Id = id;
            book.ImagePath = existingBook.ImagePath;

            if(bookContract.ImageFile != null)
            {
                if(!string.IsNullOrEmpty(existingBook.ImagePath))
                {
                    _fileService.Delete(existingBook.ImagePath);
                }

                var newImagePath = await _fileService.SaveAsync(bookContract.ImageFile);
                book.ImagePath = newImagePath;
            }


            var guid = await _booksService.UpdateAsync(book);

            return Ok(guid);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var guid = await _booksService.DeleteAsync(id);

            return Ok(guid);
        }

        [Authorize(Roles = "Admin")]
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

        [HttpGet("pages")]
        public async Task<IActionResult> GetPage(int pageIndex, int pageSize)
        {
            var books = await _booksService.GetPageAsync(pageIndex, pageSize);

            return Ok(books);
        }

        [HttpGet("amount")]
        public async Task<IActionResult> GetBooksAmount()
        {
            var amount = await _booksService.GetAmountAsync();

            return Ok(amount);
        }
    }
}
