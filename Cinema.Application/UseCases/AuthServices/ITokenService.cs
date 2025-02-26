using Cinema.Application.DTO.AuthServiceDTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.UseCases.AuthServices
{
    public interface ITokenService
    {
        Task<LoginResponseDto> AuthenticateAsync(LoginDto model);
        Task<IdentityResult> RegisterAsync(RegisterDto model);
    }
}
