using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.UseCases.AuthServices
{
    public interface IAccountService
    {
        Task RegisterAsync(string email, string userName, string password);
        Task<string> LoginAsync(string email, string password);
    }
}
