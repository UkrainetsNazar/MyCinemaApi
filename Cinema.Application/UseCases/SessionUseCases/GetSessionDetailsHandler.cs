using AutoMapper;
using Cinema.Application.DTO.SeatDTOs;
using Cinema.Application.DTO.SessionDTOs;
using Cinema.Application.Interfaces;

namespace Cinema.Application.UseCases.SessionUseCases
{
    public class GetSessionDetailsHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSessionDetailsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SessionDetailsDTO?> HandleAsync(int sessionId)
        {
            var session = await _unitOfWork.Sessions.GetByIdWithHallAndSeatsAsync(sessionId);
            if (session == null) return null;

            var sessionDetailsDTO = _mapper.Map<SessionDetailsDTO>(session);

            sessionDetailsDTO.HallNumber = session!.Hall!.NumberOfHall;

            sessionDetailsDTO.Seats = session!.Hall!.Rows!
                .SelectMany(r => r.Seats!)
                .Select(seat => new GetSeatDTO
                {
                    Id = seat.Id,
                    RowNumber = seat!.Row!.RowNumber,
                    SeatNumber = seat.SeatNumber,
                    IsBooked = seat.IsBooked
                })
                .ToList();

            return sessionDetailsDTO;
        }
    }
}
