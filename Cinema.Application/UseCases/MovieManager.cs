using Cinema.Application.UseCases.MovieUseCases;

namespace Cinema.Application.UseCases
{
    public class MovieManager
    {
        public GetAllMovieHandler? GetAllMovieHandler { get; }
        public GetMovieByIdHandler? GetMovieByIdHandler { get; }
        public AddMovieHandler? AddMovieHandler { get; }
        public UpdateMovieHandler? UpdateMovieHandler { get; }
        public DeleteMovieHandler? DeleteMovieHandler { get; }

        public MovieManager(GetAllMovieHandler getAllMovieHandler, GetMovieByIdHandler getMovieByIdHandler, AddMovieHandler addMovieHandler, UpdateMovieHandler updateMovieHandler, DeleteMovieHandler deleteMovieHandler)
        {
            GetAllMovieHandler = getAllMovieHandler;
            GetMovieByIdHandler = getMovieByIdHandler;
            AddMovieHandler = addMovieHandler;
            UpdateMovieHandler = updateMovieHandler;
            DeleteMovieHandler = deleteMovieHandler;
        }
    }
}
