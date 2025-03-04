using Cinema.Application.DTO.AuthServiceDTOs;
using Cinema.Application.UseCases.AuthServices;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Presentation.Controllers.AuthControllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _authService;
        public AuthController(IAccountService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            await _authService.RegisterAsync(model);
            return Ok("Registration succeed");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (string.IsNullOrEmpty(model.Role))
            {
                model.Role = "User";
            }

            var token = await _authService.LoginAsync(model.Email!, model.Password!, model.Role!);
            HttpContext.Response.Cookies.Append("Token", token, new CookieOptions()
            {
                HttpOnly = true
            });

            return token.Contains("Invalid") ? Unauthorized(token) : Ok(new { Token = token });
        }
    }
}
