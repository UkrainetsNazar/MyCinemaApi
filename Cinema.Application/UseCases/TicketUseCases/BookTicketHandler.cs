using AutoMapper;
using Cinema.Application.DTO.TicketDTOs;
using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;

namespace Cinema.Application.UseCases.TicketUseCases
{
    public class BookTicketHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookTicketHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task HandleAsync(CreateTicketDTO ticketDTO)
        {
            var session = await _unitOfWork.Sessions.GetByIdWithHallAndSeatsAsync(ticketDTO.SessionId);
            if (session == null)
                throw new Exception("Сеанс не знайдено.");

            var seat = session.Hall!.Rows!
                .SelectMany(r => r!.Seats!)
                .FirstOrDefault(s => s.Id == ticketDTO.SeatId);

            if (seat == null)
                throw new Exception("Місце не знайдено.");

            if (seat.IsBooked)
                throw new Exception("Це місце вже заброньоване.");

            var hallNumber = session.Hall.NumberOfHall;
            var rowNumber = seat!.Row!.RowNumber;

            var ticket = new Ticket
            {
                UserId = ticketDTO.UserId,
                SessionId = ticketDTO.SessionId,
                SeatId = ticketDTO.SeatId,
                Seat = seat,
                Session = session,
                User = await _unitOfWork.Users.GetUserByIdAsync(ticketDTO.UserId),
                HallNumber = hallNumber,
                RowNumber = rowNumber
            };

            seat.IsBooked = true;

            await _unitOfWork.Tickets.AddTicketAsync(ticket);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
