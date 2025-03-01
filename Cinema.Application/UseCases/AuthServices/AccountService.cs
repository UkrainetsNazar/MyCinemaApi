using AutoMapper;
using Cinema.Application.DTO.AuthServiceDTOs;
using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Application.UseCases.AuthServices
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountService(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        public async Task RegisterAsync(RegisterDto model)
        {
            var existingUser = await _unitOfWork.Users.GetUserByEmail(model.Email!);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            var user = _mapper.Map<RegisterDto, User>(model);

            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, model.Password!);

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
