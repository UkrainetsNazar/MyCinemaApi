using AutoMapper;
using Cinema.Application.DTO.SeatDTOs;
using Cinema.Application.DTO.SessionDTOs;
using Cinema.Application.Interfaces;

namespace Cinema.Application.UseCases.SessionUseCases
{
    public class GetAllSessionsHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllSessionsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SessionDetailsDTO>> HandleAsync()
        {
            var sessions = await _unitOfWork.Sessions.GetAllWithHallAndSeatsAsync();
            var sessionDetailsDTOs = new List<SessionDetailsDTO>();

            foreach (var session in sessions)
            {
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

                sessionDetailsDTOs.Add(sessionDetailsDTO);
            }

            return sessionDetailsDTOs;
        }
    }
}