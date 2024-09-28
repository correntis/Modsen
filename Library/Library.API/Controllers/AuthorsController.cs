using AutoMapper;
using FluentValidation;
using Library.API.Contracts;
using Library.Application.UseCases.Authors;
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
        private readonly IValidator<AuthorContract> _authorValidator;
        private readonly IMapper _mapper;

        private readonly AddAuthorUseCase _addAuthorUseCase;
        private readonly UpdateAuthorUseCase _updateAuthorUseCase;
        private readonly DeleteAuthorUseCase _deleteAuthorUseCase;
        private readonly GetAuthorUseCase _getAuthorUseCase;
        private readonly GetAllAuthorsUseCase _getAllAuthorsUseCase;
        private readonly GetAuthorsPageUseCase _getAuthorsPageUseCase;

        public AuthorsController(
            IValidator<AuthorContract> authorValidator,
            IMapper mapper,
            AddAuthorUseCase addAuthorUseCase,
            UpdateAuthorUseCase updateAuthorUseCase,
            DeleteAuthorUseCase deleteAuthorUseCase,
            GetAuthorUseCase getAuthorUseCase,
            GetAllAuthorsUseCase getAllAuthorsUseCase,
            GetAuthorsPageUseCase getAuthorsPageUseCase
            )
        {
            _authorValidator = authorValidator;
            _mapper = mapper;
            _addAuthorUseCase = addAuthorUseCase;
            _updateAuthorUseCase = updateAuthorUseCase;
            _deleteAuthorUseCase = deleteAuthorUseCase;
            _getAuthorUseCase = getAuthorUseCase;
            _getAllAuthorsUseCase = getAllAuthorsUseCase;
            _getAuthorsPageUseCase = getAuthorsPageUseCase;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(AuthorContract authorContract)
        {
            await _authorValidator.ValidateAndThrowAsync(authorContract);

            var author = _mapper.Map<Author>(authorContract);

            return Ok(await _addAuthorUseCase.ExecuteAsync(author));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, AuthorContract authorContract)
        {
            await _authorValidator.ValidateAndThrowAsync(authorContract);

            var author = _mapper.Map<Author>(authorContract);
            author.Id = id;

            await _updateAuthorUseCase.ExecuteAsync(author);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _deleteAuthorUseCase.ExecuteAsync(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _getAuthorUseCase.ExecuteAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _getAllAuthorsUseCase.ExecuteAsync());
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetPage(int pageIndex, int pageSize)
        {
            return Ok(await _getAuthorsPageUseCase.ExecuteAsync(pageIndex, pageSize));
        }
    }
}
