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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasIndex(m => m.MovieTitle)
                .IsUnique();
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Movie)
                .WithMany(m => m.Sessions)
                .HasForeignKey(s => s.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Hall)
                .WithMany(h => h.Sessions)
                .HasForeignKey(s => s.HallId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Session)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Hall>()
                .HasIndex(h => h.NumberOfHall)
                .IsUnique();
            modelBuilder.Entity<Row>()
                .HasOne(r => r.Hall)
                .WithMany(h => h.Rows)
                .HasForeignKey(r => r.HallId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Row)
                .WithMany(r => r.Seats)
                .HasForeignKey(s => s.RowId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}
