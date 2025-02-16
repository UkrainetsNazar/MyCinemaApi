using AutoMapper;
using Cinema.Application.DTO.MovieDTOs;
using Cinema.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.UseCases.SessionUseCases
{
    public class GetMovieSessionsHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMovieSessionsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<MovieSessionDTO>> HandleAsync(int movieId)
        {
            var sessions = await _unitOfWork.Sessions.GetByMovieIdAsync(movieId);

            if (sessions == null || !sessions.Any())
                return new List<MovieSessionDTO>();

            return _mapper.Map<List<MovieSessionDTO>>(sessions);
        }
    }
}
