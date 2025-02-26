using AuthService.Data;
using AuthService.DTO;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthenticationService _authService;

    public AuthController(UserManager<User> userManager, IAuthenticationService authService)
    {
        _userManager = userManager;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userInfo = await _authService.Register(model);

            return Ok(userInfo);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userInfo = await _authService.Login(model);

            return Ok(userInfo);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize]
    [HttpGet("user-info")]
    public async Task<IActionResult> GetUserInfo()
    {
        try
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.NameId);
            Console.WriteLine($"User ID from token: {userId}");

            if (userId == null)
            {
                Console.WriteLine("User ID is null in the token.");
                return BadRequest("Invalid token");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                Console.WriteLine($"User with ID {userId} not found in database.");
                return NotFound("User not found");
            }

            Console.WriteLine($"User found: {user.UserName}, Email: {user.Email}");
            return Ok(new UserInfoDto { Email = user.Email, UserName = user.UserName });
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in GetUserInfo: {e.Message}");
            return StatusCode(500, e.Message);
        }
    }
}

