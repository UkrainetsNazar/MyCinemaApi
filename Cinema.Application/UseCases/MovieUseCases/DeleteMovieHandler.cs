using AutoMapper;
using Cinema.Application.Interfaces;

namespace Cinema.Application.UseCases.MovieUseCases
{
    public class DeleteMovieHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DeleteMovieHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(int id)
        {
            var movie = await _unitOfWork.Movies.GetMovieByIdAsync(id);
            if (movie == null)
            {
                throw new Exception("Movie not found");
            }
            await _unitOfWork.Movies.DeleteMovieAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
