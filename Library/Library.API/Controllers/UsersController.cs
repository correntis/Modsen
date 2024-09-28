using AutoMapper;
using FluentValidation;
using Library.API.Contracts;
using Library.Application.UseCases.Users;
using Library.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly IValidator<UserContract> _usersValidator;
        private readonly IMapper _mapper;

        private readonly IssueUserBookUseCase _issueUserBookUseCase;
        private readonly UpdateUserUseCase _updateUserUseCase;
        private readonly DeleteUserUseCase _deleteUserUseCase;
        private readonly DeleteUserBookUseCase _deleteUserBookUseCase;
        private readonly GetUserUseCase _getUserUseCase;

        public UsersController(
            IValidator<UserContract> usersValidator,
            IMapper mapper,
            IssueUserBookUseCase issueUserBookUseCase,
            UpdateUserUseCase updateUserUseCase,
            DeleteUserUseCase deleteUserUseCase,
            DeleteUserBookUseCase deleteUserBookUseCase,
            GetUserUseCase getUserUseCase
            )
        {
            _usersValidator = usersValidator;
            _mapper = mapper;
            _issueUserBookUseCase = issueUserBookUseCase;
            _updateUserUseCase = updateUserUseCase;
            _deleteUserUseCase = deleteUserUseCase;
            _deleteUserBookUseCase = deleteUserBookUseCase;
            _getUserUseCase = getUserUseCase;
        }

        [HttpPut("{userId}/books/{bookId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> IssueBook(Guid userId, Guid bookId)
        {
            await _issueUserBookUseCase.ExecuteAsync(userId, bookId);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Update(Guid id, UserContract userContract)
        {
            await _usersValidator.ValidateAndThrowAsync(userContract);

            var user = _mapper.Map<User>(userContract);
            user.Id = id;

            await _updateUserUseCase.ExecuteAsync(user);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _deleteUserUseCase.ExecuteAsync(id);
            return Ok();
        }

        [HttpDelete("{userId}/books/{bookId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteBook(Guid userId, Guid bookId)
        {
            await _deleteUserBookUseCase.ExecuteAsync(userId, bookId);
            return Ok();
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Get(Guid userId)
        {
            return Ok(await _getUserUseCase.ExecuteAsync(userId));
        }
    }
}
