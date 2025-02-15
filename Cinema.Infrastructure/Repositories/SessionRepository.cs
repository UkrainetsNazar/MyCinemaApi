using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;
using Cinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly CinemaDbContext _context;
        public SessionRepository(CinemaDbContext context)
        {
            _context = context;
        }
        public async Task AddSessionAsync(Session session)
        {
            await _context.Sessions.AddAsync(session);
        }
        public async Task DeleteSessionAsync(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
            }
            else
            {
                throw new Exception("Session not found");
            }
        }
        public async Task<IEnumerable<Session>> GetAllSessionsAsync()
        {
            return await _context.Sessions.ToListAsync();
        }
        public async Task<Session> GetSessionByIdAsync(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                return session;
            }
            else
            {
                throw new Exception($"Session with id {id} not found");
            }
        }
        public async Task UpdateSessionAsync(int id, Session session)
        {
            var existingSession = await _context.Sessions.FirstOrDefaultAsync(s => s.Id == id);
            if (existingSession != null)
            {
                existingSession.MovieId = session.MovieId;
                existingSession.HallId = session.HallId;
                existingSession.StartTime = session.StartTime;
                existingSession.Price = session.Price;
            }
            else
            {
                throw new Exception("Session not found");
            }
        }
    }
}
