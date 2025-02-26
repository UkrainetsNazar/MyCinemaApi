using AuthService.Data;
using AuthService.DTO;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<User> _userManager;
        public TokenService(IOptions<JwtOptions> options, UserManager<User> userManager)
        {
            _jwtOptions = options.Value;
            _userManager = userManager;
        }

        public async Task<LoginResponseDto> Authenticate (LoginDto model)
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
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!)
            };

            var descriptor = new SecurityTokenDescriptor()
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials (key, SecurityAlgorithms.HmacSha256Signature)
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

        public async Task<IdentityResult> Register(RegisterDto model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                throw new Exception($"User with email {model.Email} already exists.");
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            return result;
        }
    }
}
