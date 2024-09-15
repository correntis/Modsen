using AutoMapper;
using Library.API.Contracts;
using Library.Core.Abstractions;
using Library.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public UsersController( 
            IUsersRepository usersRepository,
            IMapper mapper
            )
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        [HttpPost("{userId}/books/{bookId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> AddBook(Guid userId, Guid bookId)
        {
            var guid = await _usersRepository.AddBookAsync(userId, bookId);

            if(guid == Guid.Empty)
            {
                return StatusCode(500, "Internal Server Error");
            }

            return Ok(guid);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Update(Guid id, UserContract model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(model);
            user.Id = id;

            var guid = await _usersRepository.UpdateAsync(user);

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
            var guid = await _usersRepository.DeleteAsync(id);

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
            var guid = await _usersRepository.AddBookAsync(userId, bookId);

            if(guid == Guid.Empty)
            {
                return StatusCode(500, "Internal Server Error");
            }

            return Ok(guid);
        }
    }
}
