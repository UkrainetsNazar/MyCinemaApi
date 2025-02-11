using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.Entities
{
    public class Seat
    {
        int Id { get; set; }
        int RowId { get; set; }
        Row? Row { get; set; }
        int SeatNumber { get; set; }
        bool IsBooked { get; set; }
        int TicketId { get; set; }
        Ticket? Ticket { get; set; }
    }
}
