using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Application.UseCases.AuthServices
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        public AccountService(IUnitOfWork unitOfWork, ITokenService tokenService) 
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }
        public async Task RegisterAsync(string email, string userName, string password)
        {
            var existingUser = await _unitOfWork.Users.GetUserByEmail(email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            var user = new User
            {
                Email = email,
                UserName = userName
            };

            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, password);

            await _unitOfWork.Users.AddUser(user);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _unitOfWork.Users.GetUserByEmail(email);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var passwordVerificationResult = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash!, password);

            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {
                return _tokenService.GenerateJwtToken(user);            }
            else
            {
                throw new UnauthorizedAccessException("Invalid password");
            }
        }
    }
}
