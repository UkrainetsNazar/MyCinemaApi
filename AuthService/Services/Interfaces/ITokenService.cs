using AuthService.Data;

namespace AuthService.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
