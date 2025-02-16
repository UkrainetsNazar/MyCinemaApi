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
        Task<List<Session>> GetByMovieIdAsync(int movieId);
        Task<Session?> GetByIdWithHallAndSeatsAsync(int sessionId);
        Task<bool> IsHallAvailableAsync(int hallId, DateTime startTime, DateTime endTime);
    }
}
