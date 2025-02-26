using Cinema.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Persistence
{
    public class CinemaDbContext : IdentityDbContext<User>
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Row> Rows { get; set; }
        public DbSet<Seat> Seats { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            List<IdentityRole> roles = new()
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<Movie>()
                .HasIndex(m => m.MovieTitle)
                .IsUnique();

            builder.Entity<Session>()
                .HasOne(s => s.Movie)
                .WithMany(m => m.Sessions)
                .HasForeignKey(s => s.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Session>()
                .HasOne(s => s.Hall)
                .WithMany(h => h.Sessions)
                .HasForeignKey(s => s.HallId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Ticket>()
                .HasOne(t => t.Session)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Hall>()
                .HasIndex(h => h.NumberOfHall)
                .IsUnique();

            builder.Entity<Row>()
                .HasOne(r => r.Hall)
                .WithMany(h => h.Rows)
                .HasForeignKey(r => r.HallId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Seat>()
                .HasOne(s => s.Row)
                .WithMany(r => r.Seats)
                .HasForeignKey(s => s.RowId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Seat>()
                .HasOne(s => s.Ticket)
                .WithOne(t => t.Seat)
                .HasForeignKey<Ticket>(t => t.SeatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
