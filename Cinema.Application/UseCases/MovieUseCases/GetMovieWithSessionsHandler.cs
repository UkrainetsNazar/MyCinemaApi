using AutoMapper;
using Cinema.Application.DTO.MovieDTOs;
using Cinema.Application.DTO.SessionDTOs;
using Cinema.Application.Interfaces;

namespace Cinema.Application.UseCases.MovieUseCases
{
    public class GetMovieWithSessionsHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetMovieWithSessionsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<MovieWithSessionsDTO> HandleAsync(int movieId, DateTime date)
        {
            var movie = await _unitOfWork.Movies.GetMovieByIdAsync(movieId);
            if (movie == null)
            {
                throw new Exception($"Movie with id {movieId} not found.");
            }

            var sessions = await _unitOfWork.Sessions.GetSessionsByMovieAndDateAsync(movieId, date);

            return new MovieWithSessionsDTO
            {
                Movie = _mapper.Map<GetMovieDTO>(movie),
                Sessions = _mapper.Map<IEnumerable<GetSessionDTO>>(sessions)
            };
        }
    }
}
