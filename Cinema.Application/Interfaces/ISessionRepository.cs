using Cinema.Domain.Entities;

namespace Cinema.Application.Interfaces
{
    public interface ISessionRepository
    {
        Task<IEnumerable<Session>> GetAllSessionsAsync();
        Task<Session> GetSessionByIdAsync(int id);
        Task AddSessionAsync(Session session);
        Task UpdateSessionAsync(int id, Session session);
        Task DeleteSessionAsync(int id);
    }
}
