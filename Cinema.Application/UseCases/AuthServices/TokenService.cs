using Cinema.Application.DTO.AuthServiceDTOs;
using Cinema.Domain.Entities;
using Cinema.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cinema.Application.UseCases.AuthServices
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public TokenService(IOptions<JwtOptions> options, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _jwtOptions = options.Value;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<LoginResponseDto> AuthenticateAsync(LoginDto model)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret));

            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                throw new Exception("Email and Password are required.");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                throw new Exception("User not found.");

            var result = await _userManager.CheckPasswordAsync(user, model.Password!);
            if (!result)
                throw new Exception("Invalid password");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var descriptor = new SecurityTokenDescriptor()
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);
            Console.WriteLine("Generated Token: " + tokenHandler.WriteToken(token));
            var accessToken = tokenHandler.WriteToken(token);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email!);
            if (existingUser != null)
            {
                throw new Exception($"User with email {model.Email} already exists.");
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password!);

            if (!result.Succeeded)
            {
                throw new Exception ("User is not created");
            }

            if (!string.IsNullOrEmpty(model.Role))
            {
                var normalizedRole = model.Role.ToUpperInvariant();

                var roleExists = await _roleManager.RoleExistsAsync(normalizedRole);
                if (!roleExists)
                {
                    throw new Exception($"Role '{model.Role}' does not exist.");
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, normalizedRole);
                if (!addRoleResult.Succeeded)
                {
                    throw new Exception($"Failed to add the '{model.Role}' role to the user.");
                }
            }

            return IdentityResult.Success;
        }
    }
}
