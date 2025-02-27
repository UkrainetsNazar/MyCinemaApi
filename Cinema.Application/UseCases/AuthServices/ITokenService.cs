using Cinema.Domain.Entities;

namespace Cinema.Application.UseCases.AuthServices
{
    public interface ITokenService
    {
        Task<string> RegisterUserAsync(string email, string username, string password);
        Task<string> LoginUserAsync(string username, string password);
        string GenerateJwtToken(User user, IList<string> roles);
    }
}
