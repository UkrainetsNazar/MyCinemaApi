using Cinema.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cinema.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CinemaDbContext _context;
        private IDbContextTransaction _transaction;
        public IHallRepository Halls { get; }
        public IMovieRepository Movies { get; }
        public ISessionRepository Sessions { get; }
        public ISeatRepository Seats { get; }
        public ITicketRepository Tickets { get; }

        public UnitOfWork(CinemaDbContext context, IMovieRepository movieRepository, ISessionRepository sessionRepository, IHallRepository hallRepository, ITicketRepository ticketRepository, ISeatRepository seatRepository)
        {
            _context = context;
            Movies = movieRepository;
            Sessions = sessionRepository;
            Halls = hallRepository;
            Tickets = ticketRepository;
            Seats = seatRepository;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                _transaction.Dispose();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                _transaction.Dispose();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose() 
        {
            _context.Dispose();
        }
    }
}
