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
            var guid = await _usersService.AddBookAsync(userId, bookId);

            if(guid == Guid.Empty)
            {
                return StatusCode(500, "Internal Server Error");
            }

            return Ok(guid);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Update(Guid id, UserContract userContract)
        {
            var validationResult = await _usersValidator.ValidateAsync(userContract);

            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = _mapper.Map<User>(userContract);
            user.Id = id;

            var guid = await _usersService.UpdateAsync(user);

            if (guid == Guid.Empty)
            {
                return StatusCode(500, "Internal Server Error");
            }

            return Ok(guid);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var guid = await _usersService.DeleteAsync(id);

            if(guid == Guid.Empty)
            {
                return StatusCode(500, "Internal Server Error");
            }

            return Ok(guid);
        }

        [HttpDelete("{userId}/books/{bookId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteBook(Guid userId, Guid bookId)
        {
            var guid = await _usersService.AddBookAsync(userId, bookId);

            if(guid == Guid.Empty)
            {
                return StatusCode(500, "Internal Server Error");
            }

            return Ok(guid);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var user = await _usersService.GetAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
