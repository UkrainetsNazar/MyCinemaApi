using AutoMapper;
using Cinema.Application.DTO.AuthServiceDTOs;
using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Application.UseCases.AuthServices
{
    public class AccountService : IAccountService
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public AccountService(ITokenService tokenService, IMapper mapper, UserManager<User> userManager) 
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task RegisterAsync(RegisterDto model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email!);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            var user = _mapper.Map<RegisterDto, User>(model);

            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, model.Password!);

            await _userManager.CreateAsync(user, model.Password!);
        }
        public async Task<string> LoginAsync(string email, string password, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            await _userManager.AddToRoleAsync(user, role);

            var passwordVerificationResult = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash!, password);

            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {
                return _tokenService.GenerateJwtToken(user, role);            
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid password");
            }
        }
    }
}
