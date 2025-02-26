using AuthService.Data;
using AuthService.DTO;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services.Interfaces
{
    public interface ITokenService
    {
        Task<LoginResponseDto> Authenticate(LoginDto model);
        Task<IdentityResult> Register(RegisterDto model);
    }
}
