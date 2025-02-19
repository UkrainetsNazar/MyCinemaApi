using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;

namespace Cinema.Application.UseCases.TicketUseCases
{
    public class BuyTicketHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public BuyTicketHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Ticket> HandleAsync(int sessionId, int seatId, int userId)
        {
            var session = await _unitOfWork.Sessions.GetSessionByIdAsync(sessionId);
            if (session == null)
            {
                throw new Exception($"Session with id {sessionId} not found.");
            }

            var seat = await _unitOfWork.Seats.GetSeatByIdAsync(seatId);
            if (seat == null)
            {
                throw new Exception($"Seat with id {seatId} not found.");
            }

            if (seat.IsBooked)
            {
                throw new Exception($"Seat with id {seatId} is already booked.");
            }

            var user = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception($"User with id {userId} not found.");
            }


            var ticket = new Ticket
            {
                SessionId = sessionId,
                UserId = userId,
                SeatId = seatId,
                Price = session.Price,
                SessionTime = session.StartTime,
                Seat = seat,
                Session = session,
                User = user
            };

            seat.IsBooked = true;

            await _unitOfWork.BeginTransactionAsync();
            
            try
            {
                await _unitOfWork.Tickets.AddTicketAsync(ticket);

                await _unitOfWork.Seats.UpdateSeatAsync(seatId, seat);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return ticket;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("An error occurred while processing the ticket purchase.", ex);
            }
        }
    }
}
