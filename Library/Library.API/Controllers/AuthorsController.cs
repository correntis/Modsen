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
            await _authorValidator.ValidateAndThrowAsync(authorContract);

            var author = _mapper.Map<Author>(authorContract);

            return Ok(await _authorsService.AddAsync(author));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, AuthorContract authorContract)
        {
            await _authorValidator.ValidateAndThrowAsync(authorContract);

            var author = _mapper.Map<Author>(authorContract);
            author.Id = id;

            await _authorsService.UpdateAsync(author);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _authorsService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _authorsService.GetAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _authorsService.GetAllAsync());
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetPage(int pageIndex, int pageSize)
        {
            return Ok(await _authorsService.GetPageAsync(pageIndex, pageSize));
        }
    }
}
