using AuthService.Data;
using AuthService.DTO;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationService(ITokenService tokenService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserInfoDto> Register(RegisterDto model)
        {
            var user = new User { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password!);

            if (!result.Succeeded)
            {
                Console.WriteLine($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var role = string.IsNullOrEmpty(model.Role) ? "User" : model.Role;
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                throw new Exception($"Role {role} does not exist.");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, role);

            if (!roleResult.Succeeded)
            {
                Console.WriteLine($"Failed to add role to user: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                throw new Exception(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }

            Console.WriteLine($"User {user.UserName} registered successfully with ID {user.Id}");
            return new UserInfoDto { Email = model.Email, Role = role, UserName = model.UserName };
        }


        public async Task<UserInfoDto> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email!);

            if (user == null)
                throw new Exception("User not found");

            var result = await _userManager.CheckPasswordAsync(user, model.Password!);

            if (!result)
                throw new Exception("Invalid password");

            var token = _tokenService.CreateToken(user);

            return new UserInfoDto { Email = user.Email, UserName = user.UserName, Token = token };
        }
    }
}
