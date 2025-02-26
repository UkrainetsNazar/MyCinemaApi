using Cinema.Application.DTO.AuthServiceDTOs;
using Cinema.Application.UseCases.AuthServices;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Presentation.Controllers.AuthControllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                var result = await _tokenService.RegisterAsync(model);
                if (result == null)
                    return BadRequest("Failed to register user");

                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var userInfo = await _tokenService.AuthenticateAsync(model);
                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
