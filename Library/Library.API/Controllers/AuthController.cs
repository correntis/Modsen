using AutoMapper;
using FluentValidation;
using Library.API.Contracts;
using Library.Core.Exceptions;
using Library.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<RegisterContract> _registerValidator;
        private readonly IValidator<LoginContract> _loginValidator;

        public AuthController(
            IAuthService authService,
            IValidator<RegisterContract> registerValidator,
            IValidator<LoginContract> loginValidator
            )
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterContract model)
        {
            await _registerValidator.ValidateAndThrowAsync(model);

            var userTokens = await _authService.Register(model.UserName, model.Email, model.Password);

            AppendCookies("accessToken", userTokens.AccessToken);
            AppendCookies("refreshToken", userTokens.RefreshToken);

            return Ok(userTokens.UserId);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginContract model)
        {
            await _loginValidator.ValidateAndThrowAsync(model);

            var (user, userTokens) = await _authService.Login(model.Email,model.Password);

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
