using AutoMapper;
using Cinema.Application.DTO.MovieDTOs;
using Cinema.Application.Interfaces;

namespace Cinema.Application.UseCases.MovieUseCases
{
    public class GetMoviesByDateHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetMoviesByDateHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetMovieDTO>> HandleAsync(DateTime date)
        {
            var movies = await _unitOfWork.Movies.GetMoviesByDateAsync(date);
            if (!movies.Any())
            {
                throw new Exception($"No movies found for date {date.ToShortDateString()}.");
            }

            return _mapper.Map<IEnumerable<GetMovieDTO>>(movies);
        }
    }
}
