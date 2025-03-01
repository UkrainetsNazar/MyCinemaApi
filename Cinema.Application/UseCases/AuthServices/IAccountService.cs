using Cinema.Application.DTO.AuthServiceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.UseCases.AuthServices
{
    public interface IAccountService
    {
        Task RegisterAsync(RegisterDto model);
        Task<string> LoginAsync(string email, string password);
    }
}
