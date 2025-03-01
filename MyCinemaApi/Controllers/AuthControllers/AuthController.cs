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
            await _authService.RegisterAsync(model.Email!, model.UserName!, model.Password!);
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var token = await _authService.LoginAsync(model.Email!, model.Password!);
            return token.Contains("Invalid") ? Unauthorized(token) : Ok(new { Token = token });
        }
    }
}
