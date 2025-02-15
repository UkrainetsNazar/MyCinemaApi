using AutoMapper;
using Cinema.Application.DTO.MovieDTOs;
using Cinema.Application.Interfaces;

namespace Cinema.Application.UseCases.MovieUseCases
{
    public class GetMovieByIdHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetMovieByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetMovieDTO> HandleAsync(int id)
        {
            var movie = await _unitOfWork.Movies.GetMovieByIdAsync(id);
            if (movie == null)
            {
                throw new Exception("Movie not found");
            }
            return _mapper.Map<GetMovieDTO>(movie);
        }
    }
}
