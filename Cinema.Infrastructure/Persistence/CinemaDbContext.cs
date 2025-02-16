using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Persistence
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options) { }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Row> Rows { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<User> Users { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentTime = DateTime.Now;

            var sessions = await Sessions.ToListAsync(cancellationToken);

            var expiredSessions = sessions.Where(s =>
            {
                var movie = s.Movie;
                if (movie == null)
                    return false; 

                var endTime = s.StartTime.AddMinutes(movie.DurationMinutes);
                return endTime < currentTime;
            }).ToList();

            Sessions.RemoveRange(expiredSessions);

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
