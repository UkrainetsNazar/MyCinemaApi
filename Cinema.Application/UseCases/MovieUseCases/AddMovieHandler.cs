using AutoMapper;
using Cinema.Application.DTO.MovieDTOs;
using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;

namespace Cinema.Application.UseCases.MovieUseCases
{
    public class AddMovieHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddMovieHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(CreateMovieDTO movieDTO)
        {
            var movie = _mapper.Map<Movie>(movieDTO);
            await _unitOfWork.Movies.AddMovieAsync(movie);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
