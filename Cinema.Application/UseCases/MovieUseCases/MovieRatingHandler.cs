using AutoMapper;
using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.UseCases.MovieUseCases
{
    public class MovieRatingHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MovieRatingHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Movie> HandleAsync(int movieId, double rating)
        {
            var movie = await _unitOfWork.Movies.GetMovieByIdAsync(movieId);
            if (movie == null)
                throw new Exception("Movie not found");

            movie.Rating += rating;
            movie.RatingCount++;

            movie.Rating = movie.Rating / movie.RatingCount;

            await _unitOfWork.Movies.UpdateMovieAsync(movieId, movie);
            await _unitOfWork.SaveChangesAsync();
            return movie;
        }
    }
}
