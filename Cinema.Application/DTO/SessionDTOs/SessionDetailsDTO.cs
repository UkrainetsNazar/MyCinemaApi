using Cinema.Application.DTO.SeatDTOs;

namespace Cinema.Application.DTO.SessionDTOs
{
    public class SessionDetailsDTO
    {
        public int SessionId { get; set; }
        public DateTime StartTime { get; set; }
        public double Price { get; set; }
        public int HallNumber { get; set; }
        public List<GetSeatDTO> Seats { get; set; }
    }
}
