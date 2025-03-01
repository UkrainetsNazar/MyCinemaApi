using Cinema.Domain.Entities;

namespace Cinema.Application.UseCases.AuthServices
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}
