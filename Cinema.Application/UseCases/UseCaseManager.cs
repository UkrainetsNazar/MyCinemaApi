using Cinema.Application.UseCases.HallUseCases;
using Cinema.Application.UseCases.MovieUseCases;
using Cinema.Application.UseCases.SessionUseCases;
using Cinema.Application.UseCases.TicketUseCases;

namespace Cinema.Application.UseCases
{
    public class UseCaseManager
    {
        public AddMovieHandler AddMovieHandler { get; }
        public GetMovieWithSessionsHandler GetMovieWithSessionsHandler { get; }
        public GetMoviesByDateHandler GetMoviesByDateHandler { get; }
        public UpdateMovieHandler UpdateMovieHandler { get; }
        public DeleteMovieHandler DeleteMovieHandler { get; }
        public CreateHallHandler CreateHallHandler { get; }
        public MovieRatingHandler MovieRatingHandler { get; }
        public GetUserTicketsHandler GetUserTicketsHandler { get; }
        public BuyTicketHandler BuyTicketHandler { get; }
        public AddSessionHandler AddSessionHandler { get; }
        public GetSessionDetailsHandler GetSessionDetailsHandler { get; }
        public GetAllSessionsHandler GetAllSessionsHandler { get; }
        public UseCaseManager(AddMovieHandler addMovieHandler, GetMovieWithSessionsHandler getMovieWithSessionsHandler, GetMoviesByDateHandler getMoviesByDateHandler, UpdateMovieHandler updateMovieHandler, DeleteMovieHandler deleteMovieHandler, MovieRatingHandler movieRatingHandler,
            CreateHallHandler createHallHandler, GetUserTicketsHandler getUserTicketsHandler, BuyTicketHandler buyTicketHandler, AddSessionHandler addSessionHandler, GetSessionDetailsHandler getSessionDetailsHandler, GetAllSessionsHandler getAllSessionsHandler)
        {
            AddMovieHandler = addMovieHandler;
            GetMovieWithSessionsHandler = getMovieWithSessionsHandler;
            GetMoviesByDateHandler = getMoviesByDateHandler;
            UpdateMovieHandler = updateMovieHandler;
            DeleteMovieHandler = deleteMovieHandler;
            CreateHallHandler = createHallHandler;
            MovieRatingHandler = movieRatingHandler;
            GetUserTicketsHandler = getUserTicketsHandler;
            BuyTicketHandler = buyTicketHandler;
            AddSessionHandler = addSessionHandler;
            GetSessionDetailsHandler = getSessionDetailsHandler;
            GetAllSessionsHandler = getAllSessionsHandler;
        }

    }
}
