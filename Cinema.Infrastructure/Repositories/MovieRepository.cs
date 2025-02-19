using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;
using Cinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cinema.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly CinemaDbContext _context;
        private readonly ILogger<MovieRepository> _logger;

        public MovieRepository(CinemaDbContext context, ILogger<MovieRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddMovieAsync(Movie movie)
        {
            try
            {
                _logger.LogInformation("Adding movie: {MovieTitle}", movie.MovieTitle);
                await _context.Movies.AddAsync(movie);
                _logger.LogInformation("Movie added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding movie: {MovieTitle}", movie.MovieTitle);
                throw;
            }
        }

        public async Task DeleteMovieAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting movie with id {MovieId}", id);
                var movie = await _context.Movies.FindAsync(id);
                if (movie != null)
                {
                    _context.Movies.Remove(movie);
                    _logger.LogInformation("Movie with id {MovieId} deleted successfully", id);
                }
                else
                {
                    _logger.LogWarning("Movie with id {MovieId} not found", id);
                    throw new Exception("Movie not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting movie with id {MovieId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all movies");
                var movies = await _context.Movies.ToListAsync();
                _logger.LogInformation("Fetched {MovieCount} movies", movies.Count);
                return movies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all movies");
                throw;
            }
        }

        public async Task<IEnumerable<Movie>> GetMoviesByDateAsync(DateTime date)
        {
            return await _context.Movies
                .Where(ms => ms.StartDate <= date && ms.EndDate >= date)
                .Distinct()
                .ToListAsync();
        }


        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching movie with id {MovieId}", id);
                var movie = await _context.Movies.FindAsync(id);

                if (movie != null)
                {
                    _logger.LogInformation("Movie with id {MovieId} found", id);
                    return movie;
                }
                else
                {
                    _logger.LogWarning("Movie with id {MovieId} not found", id);
                    throw new Exception($"Movie with id {id} not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching movie with id {MovieId}", id);
                throw;
            }
        }


        public async Task UpdateMovieAsync(int id, Movie movie)
        {
            try
            {
                _logger.LogInformation("Updating movie with id {MovieId}", id);
                var existingMovie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
                if (existingMovie != null)
                {
                    existingMovie.MovieTitle = movie.MovieTitle;
                    existingMovie.Description = movie.Description;
                    existingMovie.DurationMinutes = movie.DurationMinutes;
                    existingMovie.Genre = movie.Genre;
                    existingMovie.PosterUrl = movie.PosterUrl;
                    existingMovie.TrailerUrl = movie.TrailerUrl;
                    existingMovie.ReleaseDate = movie.ReleaseDate;
                    existingMovie.RatingCount = movie.RatingCount;
                    existingMovie.Rating = movie.Rating;
                    existingMovie.StartDate = movie.StartDate;
                    existingMovie.EndDate = movie.EndDate;

                    _logger.LogInformation("Movie with id {MovieId} updated successfully", id);
                }
                else
                {
                    _logger.LogWarning("Movie with id {MovieId} not found", id);
                    throw new Exception("Movie not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating movie with id {MovieId}", id);
                throw;
            }
        }
    }
}
