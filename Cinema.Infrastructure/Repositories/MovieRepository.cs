using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;
using Cinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly CinemaDbContext _context;
        public MovieRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task AddMovieAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
        }

        public async Task DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }
            else
            {
                throw new Exception("Movie not found");
            }
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                return movie;
            }
            else
            {
                throw new Exception($"Movie with id {id} not found");
            }
        }

        public async Task UpdateMovieAsync(int id, Movie movie)
        {
            var existingMovie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            if (existingMovie != null)
            {
                existingMovie.MovieTitle = 
                existingMovie.Description = movie.Description;
                existingMovie.DurationMinutes = movie.DurationMinutes;
                existingMovie.Genre = movie.Genre;
                existingMovie.PosterUrl = movie.PosterUrl;
                existingMovie.TrailerUrl = movie.TrailerUrl;
                existingMovie.ReleaseDate = movie.ReleaseDate;
                existingMovie.Rating = movie.Rating;
            }
        }
    }
}
