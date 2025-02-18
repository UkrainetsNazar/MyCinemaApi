using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int HallNumber { get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }
        public double Price { get; set; }
        public DateTime SessionTime { get; set; }
    }
}
