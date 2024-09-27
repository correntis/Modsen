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
            await _usersService.IssueBookAsync(userId, bookId);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Update(Guid id, UserContract userContract)
        {
            await _usersValidator.ValidateAndThrowAsync(userContract);

            var user = _mapper.Map<User>(userContract);
            user.Id = id;

            await _usersService.UpdateAsync(user);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _usersService.DeleteAsync(id);
            return Ok();
        }

        [HttpDelete("{userId}/books/{bookId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteBook(Guid userId, Guid bookId)
        {
            await _usersService.DeleteBookAsync(userId, bookId);
            return Ok();
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Get(Guid userId)
        {
            return Ok(await _usersService.GetAsync(userId));
        }
    }
}
