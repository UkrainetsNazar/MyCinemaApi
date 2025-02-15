using AutoMapper;
using Cinema.Application.DTO.MovieDTOs;
using Cinema.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.UseCases.MovieUseCases
{
    public class UpdateMovieHandler 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UpdateMovieHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(int id, UpdateMovieDTO movieDTO)
        {
            var movie = await _unitOfWork.Movies.GetMovieByIdAsync(id);
            if (movie == null)
            {
                throw new Exception("Movie not found");
            }
            _mapper.Map(movieDTO, movie);
            await _unitOfWork.Movies.UpdateMovieAsync(id, movie);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
