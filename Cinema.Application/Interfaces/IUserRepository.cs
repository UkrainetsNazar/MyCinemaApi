using Cinema.Domain.Entities;

namespace Cinema.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task UpdateUserAsync(string id, User user);
        Task DeleteUserAsync(string id);
    }
}
