using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;
using Cinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cinema.Infrastructure.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly CinemaDbContext _context;
        private readonly ILogger<SessionRepository> _logger;

        public SessionRepository(CinemaDbContext context, ILogger<SessionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddSessionAsync(Session session)
        {
            try
            {
                _logger.LogInformation("Adding session for movieId {MovieId} in hallId {HallId} at {StartTime}", session.MovieId, session.HallId, session.StartTime);
                await _context.Sessions.AddAsync(session);
                _logger.LogInformation("Session added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding session for movieId {MovieId}", session.MovieId);
                throw;
            }
        }



        public async Task DeleteSessionAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting session with id {SessionId}", id);
                var session = await _context.Sessions.FindAsync(id);
                if (session != null)
                {
                    _context.Sessions.Remove(session);
                    _logger.LogInformation("Session with id {SessionId} deleted successfully", id);
                }
                else
                {
                    _logger.LogWarning("Session with id {SessionId} not found", id);
                    throw new Exception("Session not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting session with id {SessionId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Session>> GetAllWithHallAndSeatsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all sessions including hall and seats");
                var sessions = await _context.Sessions
                    .Include(s => s.Hall!)
                    .ThenInclude(h => h.Rows!)
                    .ThenInclude(r => r.Seats)
                    .ToListAsync();

                _logger.LogInformation("Fetched {Count} sessions", sessions.Count);
                return sessions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all sessions");
                throw;
            }
        }

        public async Task<Session> GetSessionByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching session with id {SessionId}", id);
                var session = await _context.Sessions.FindAsync(id);
                if (session != null)
                {
                    _logger.LogInformation("Session with id {SessionId} found", id);
                    return session;
                }
                else
                {
                    _logger.LogWarning("Session with id {SessionId} not found", id);
                    throw new Exception($"Session with id {id} not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching session with id {SessionId}", id);
                throw;
            }
        }

        public async Task UpdateSessionAsync(int id, Session session)
        {
            try
            {
                _logger.LogInformation("Updating session with id {SessionId}", id);
                var existingSession = await _context.Sessions.FirstOrDefaultAsync(s => s.Id == id);
                if (existingSession != null)
                {
                    existingSession.MovieId = session.MovieId;
                    existingSession.HallId = session.HallId;
                    existingSession.StartTime = session.StartTime;
                    existingSession.Price = session.Price;
                    _logger.LogInformation("Session with id {SessionId} updated successfully", id);
                }
                else
                {
                    _logger.LogWarning("Session with id {SessionId} not found", id);
                    throw new Exception("Session not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating session with id {SessionId}", id);
                throw;
            }
        }

        public async Task<Session?> GetByIdWithHallAndSeatsAsync(int sessionId)
        {
            try
            {
                _logger.LogInformation("Fetching session with id {SessionId} including hall and seats", sessionId);
                var session = await _context.Sessions
                    .Include(s => s.Hall!)
                    .ThenInclude(h => h.Rows!)
                    .ThenInclude(r => r.Seats)
                    .FirstOrDefaultAsync(s => s.Id == sessionId);

                if (session != null)
                {
                    _logger.LogInformation("Session with id {SessionId} found", sessionId);
                }
                else
                {
                    _logger.LogWarning("Session with id {SessionId} not found", sessionId);
                }

                return session;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching session with id {SessionId}", sessionId);
                throw;
            }
        }

        public async Task<bool> IsHallAvailableAsync(int hallId, DateTime startTime, DateTime endTime)
        {
            try
            {
                _logger.LogInformation("Checking availability for hallId {HallId} from {StartTime} to {EndTime}", hallId, startTime, endTime);
                bool isAvailable = !await _context.Sessions
                    .AnyAsync(s => s.HallId == hallId &&
                                   (startTime < s.StartTime.AddMinutes(s.Movie!.DurationMinutes) &&
                                    endTime > s.StartTime));

                _logger.LogInformation("HallId {HallId} availability check result: {IsAvailable}", hallId, isAvailable);
                return isAvailable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking availability for hallId {HallId}", hallId);
                throw;
            }
        }

        public async Task<IEnumerable<Session>> GetSessionsByMovieAndDateAsync(int movieId, DateTime date)
        {
            return await _context.Sessions
                .Where(s => s.MovieId == movieId && s.StartTime.Date == date.Date)
                .ToListAsync();
        }
    }
}
