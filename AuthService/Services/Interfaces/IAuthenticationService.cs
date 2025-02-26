using AuthService.DTO;

namespace AuthService.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserInfoDto> Login(LoginDto loginDto);
        Task<UserInfoDto> Register(RegisterDto registerDto);
    }
}
