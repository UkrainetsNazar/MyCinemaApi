using AuthService.DTO;
using Cinema.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = new User { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password!);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        var roleResult = await _userManager.AddToRoleAsync(user, "User"); 
        if (!roleResult.Succeeded)
            return BadRequest(roleResult.Errors);

        return Ok("Registration successful");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, false, false);

        if (!result.Succeeded)
            return Unauthorized();

        return Ok("Login successful");
    }
}

