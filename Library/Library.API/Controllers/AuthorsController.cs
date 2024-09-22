using AutoMapper;
using FluentValidation;
using Library.API.Contracts;
using Library.Core.Abstractions;
using Library.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/v1/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ILogger<AuthorsController> _logger;
        private readonly IAuthorsService _authorsService;
        private readonly IMapper _mapper;
        private readonly IValidator<AuthorContract> _authorValidator;

        public AuthorsController(
            ILogger<AuthorsController> logger,
            IAuthorsService authorsService,
            IMapper mapper,
            IValidator<AuthorContract> authorValidator
            )
        {
            _logger = logger;
            _authorsService = authorsService;
            _mapper = mapper;
            _authorValidator = authorValidator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(AuthorContract authorContract)
        {
            var validationResult = await _authorValidator.ValidateAsync(authorContract);

            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var author = _mapper.Map<Author>(authorContract);

            var guid = await _authorsService.AddAsync(author);

            return Ok(guid);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, AuthorContract authorContract)
        {
            var validationResult = await _authorValidator.ValidateAsync(authorContract);

            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var author = _mapper.Map<Author>(authorContract);
            author.Id = id;

            var guid = await _authorsService.UpdateAsync(author);

            return Ok(guid);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var guid = await _authorsService.DeleteAsync(id);

            return Ok(guid);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var author = await _authorsService.GetAsync(id);

            return Ok(author);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _authorsService.GetAllAsync();

            return Ok(authors);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetPage(int pageIndex, int pageSize)
        {
            var authors = await _authorsService.GetPageAsync(pageIndex, pageSize);

            return Ok(authors);
        }
    }
}
