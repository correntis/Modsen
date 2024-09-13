using AutoMapper;
using Library.API.Contracts;
using Library.Core.Abstractions;
using Library.Core.Models;
using Library.DataAccess.Configuration;
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

        public AuthorsController(
            ILogger<AuthorsController> logger,
            IAuthorsService authorsService,
            IMapper mapper
            )
        {
            _logger = logger;
            _authorsService = authorsService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorContract authorContract)
        {
            var author = _mapper.Map<Author>(authorContract);

            var guid = await _authorsService.AddAsync(author);

            return Ok(guid);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, AuthorContract authorContract)
        {
            var author = _mapper.Map<Author>(authorContract);
            author.Id = id;

            var guid = await _authorsService.UpdateAsync(author);

            return Ok(guid);
        }

        [HttpDelete("{id}")]
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
