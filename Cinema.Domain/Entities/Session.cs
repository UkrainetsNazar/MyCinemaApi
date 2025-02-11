using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.Entities
{
    public class Session
    {
        int Id { get; set; }
        DateTime StartTime { get; set; }
        int MovieId { get; set; }
        Movie? Movie { get; set; }
        int HallId { get; set; }
        Hall? Hall { get; set; }
        List<Ticket>? Tickets { get; set; }
        double Price { get; set; }
    }
}
