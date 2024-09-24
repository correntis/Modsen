using AutoMapper;
using FluentValidation;
using Library.API.Contracts;
using Library.API.Validation;
using Library.Core.Abstractions;
using Library.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;
        private readonly IValidator<UserContract> _usersValidator;

        public UsersController( 
            IUsersService usersService,
            IMapper mapper,
            IValidator<UserContract> usersValidator
            )
        {
            _usersService = usersService;
            _mapper = mapper;
            _usersValidator = usersValidator;
        }

        [HttpPut("{userId}/books/{bookId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> AddBook(Guid userId, Guid bookId)
        {
            return Ok(await _usersService.AddBookAsync(userId, bookId));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Update(Guid id, UserContract userContract)
        {
            await _usersValidator.ValidateAndThrowAsync(userContract);

            var user = _mapper.Map<User>(userContract);
            user.Id = id;

            return Ok(await _usersService.UpdateAsync(user));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _usersService.DeleteAsync(id));
        }

        [HttpDelete("{userId}/books/{bookId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteBook(Guid userId, Guid bookId)
        {
            return Ok(await _usersService.AddBookAsync(userId, bookId));
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Get(Guid userId)
        {
            return Ok(await _usersService.GetAsync(userId));
        }
    }
}
