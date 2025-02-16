using AutoMapper;
using Cinema.Application.DTO.SessionDTOs;
using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;

namespace Cinema.Application.UseCases.SessionUseCases
{
    public class AddSessionHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddSessionHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(CreateSessionDTO sessionDTO)
        {
            var movie = await _unitOfWork.Movies.GetMovieByIdAsync(sessionDTO.MovieId);
            if (movie == null)
                throw new Exception("Фільм не знайдено.");

            var existingHall = await _unitOfWork.Halls.GetByNumberAsync(sessionDTO.NumberOfHall);
            if (existingHall == null)
            {
                throw new Exception($"Зал №{sessionDTO.NumberOfHall} не знайдено.");
            }

            var isHallAvailable = await _unitOfWork.Sessions.IsHallAvailableAsync(existingHall.Id, sessionDTO.StartTime, sessionDTO.StartTime.AddMinutes(movie.DurationMinutes));
            if (!isHallAvailable)
            {
                throw new Exception("Зал вже зайнятий на цей час.");
            }

            var session = _mapper.Map<Session>(sessionDTO);
            session.HallId = existingHall.Id;
            session.Hall = existingHall;
            session.Movie = movie;

            await _unitOfWork.Sessions.AddSessionAsync(session);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
