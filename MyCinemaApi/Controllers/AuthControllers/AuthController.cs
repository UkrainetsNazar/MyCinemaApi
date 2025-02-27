using Cinema.Application.DTO.AuthServiceDTOs;
using Cinema.Application.UseCases.AuthServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Presentation.Controllers.AuthControllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _authService;
        public AuthController(ITokenService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = await _authService.RegisterUserAsync(model.Email!, model.UserName!, model.Password!);
            return result.Contains("successfully") ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var token = await _authService.LoginUserAsync(model.Email!, model.Password!);
            return token.Contains("Invalid") ? Unauthorized(token) : Ok(new { Token = token });
        }
    }
}
