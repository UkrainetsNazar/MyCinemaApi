using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public Session? Session { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int SeatId { get; set; }
        public Seat? Seat { get; set; }

        public double Price { get; set; }
        public DateTime SessionTime { get; set; }

        [NotMapped]
        public int HallNumber => Seat.Row.Hall.NumberOfHall;

        [NotMapped]
        public int RowNumber => Seat.Row.RowNumber;

        [NotMapped]
        public int SeatNumber => Seat.SeatNumber;
    }
 }
