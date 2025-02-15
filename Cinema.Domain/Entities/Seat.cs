using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.Entities
{
    public class Seat
    {
        public int Id { get; set; }
        public int RowId { get; set; }
        public Row? Row { get; set; }
        public int SeatNumber { get; set; }
        public bool IsBooked { get; set; }
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }
    }
}
