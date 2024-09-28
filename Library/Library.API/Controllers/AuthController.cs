using FluentValidation;
using Library.API.Contracts;
using Library.Application.UseCases.Auth;
using Library.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IValidator<RegisterContract> _registerValidator;
        private readonly IValidator<LoginContract> _loginValidator;

        private readonly RegisterUserUseCase _registerUserUseCase;
        private readonly LoginUserUseCase _loginUserUseCase;

        public AuthController(
            IValidator<RegisterContract> registerValidator,
            IValidator<LoginContract> loginValidator,
            RegisterUserUseCase registerUserUseCase,
            LoginUserUseCase loginUserUseCase
            )
        {
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _registerUserUseCase = registerUserUseCase;
            _loginUserUseCase = loginUserUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterContract model)
        {
            await _registerValidator.ValidateAndThrowAsync(model);

            var userTokens = await _registerUserUseCase.ExecuteAsync(model.UserName, model.Email, model.Password);

            AppendCookies("accessToken", userTokens.AccessToken);
            AppendCookies("refreshToken", userTokens.RefreshToken);

            return Ok(userTokens.UserId);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginContract model)
        {
            await _loginValidator.ValidateAndThrowAsync(model);

            var (user, userTokens) = await _loginUserUseCase.ExecuteAsync(model.Email,model.Password);

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
