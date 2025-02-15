using AutoMapper;
using Cinema.Application.DTO.MovieDTOs;
using Cinema.Application.Interfaces;

namespace Cinema.Application.UseCases.MovieUseCases
{
    public class GetAllMovieHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllMovieHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetMovieDTO>> HandleAsync()
        {
            var movies = await _unitOfWork.Movies.GetAllMoviesAsync();
            return _mapper.Map<IEnumerable<GetMovieDTO>>(movies);
        }
    }
}
