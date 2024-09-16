using AutoMapper;
using Library.API.Contracts;
using Library.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(
            IAuthService authService,
            IMapper mapper
            )
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterContract model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userTokens = await _authService.Register(model.UserName, model.Email, model.Password);

            if (userTokens == null)
            {
                return StatusCode(500, "Internal Server Error");
            }

            AppendCookies("accessToken", userTokens.AccessToken);
            AppendCookies("refreshToken", userTokens.RefreshToken);

            return Ok(userTokens.UserId);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginContract model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (user, userTokens) = await _authService.Login(model.Email,model.Password);

            if(user == null || userTokens == null)
            {
                return StatusCode(500, "Internal Server Error");
            }

            AppendCookies("accessToken", userTokens.AccessToken);           
            AppendCookies("refreshToken", userTokens.RefreshToken);

            return Ok(user);
        }

        private void AppendCookies(string name, string value)
        {
            var options = new CookieOptions() { Expires = DateTime.UtcNow.AddDays(14) };

            HttpContext.Response.Cookies.Append(name, value, options);
        }
    }
}
