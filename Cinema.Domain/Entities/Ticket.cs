using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.Entities
{
    public class Ticket
    {
        int Id { get; set; }
        int SessionId { get; set; }
        Session? Session { get; set; }
        int UserId { get; set; }
        User? User { get; set; }
        int SeatId { get; set; }
        Seat? Seat { get; set; }
    }
}
