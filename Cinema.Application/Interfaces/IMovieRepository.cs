using Cinema.Domain.Entities;

namespace Cinema.Application.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<Movie> GetMovieByIdAsync(int id);
        Task AddMovieAsync(Movie movie);
        Task UpdateMovieAsync(int id, Movie movie);
        Task DeleteMovieAsync(int id);
    }
}
