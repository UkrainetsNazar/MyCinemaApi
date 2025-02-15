using Cinema.Application.Interfaces;

namespace Cinema.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CinemaDbContext _context;
        public IUserRepository Users { get; }
        public IHallRepository Halls { get; }
        public IMovieRepository Movies { get; }
        public ISessionRepository Sessions { get; }
        public ITicketRepository Tickets { get; }

        public UnitOfWork(CinemaDbContext context, IMovieRepository movieRepository, ISessionRepository sessionRepository, IHallRepository hallRepository, ITicketRepository ticketRepository, IUserRepository userRepository)
        {
            _context = context;
            Movies = movieRepository;
            Sessions = sessionRepository;
            Halls = hallRepository;
            Tickets = ticketRepository;
            Users = userRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose() {
            _context.Dispose();
        }
    }
}
