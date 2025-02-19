using Cinema.Domain.Entities;

namespace Cinema.Application.Interfaces
{
    public interface ISessionRepository
    {
        Task<IEnumerable<Session>> GetAllWithHallAndSeatsAsync();
        Task<Session> GetSessionByIdAsync(int id);
        Task AddSessionAsync(Session session);
        Task UpdateSessionAsync(int id, Session session);
        Task DeleteSessionAsync(int id);
        Task<IEnumerable<Session>> GetSessionsByMovieAndDateAsync(int movieId, DateTime date);
        Task<Session?> GetByIdWithHallAndSeatsAsync(int sessionId);
        Task<bool> IsHallAvailableAsync(int hallId, DateTime startTime, DateTime endTime);
    }
}
