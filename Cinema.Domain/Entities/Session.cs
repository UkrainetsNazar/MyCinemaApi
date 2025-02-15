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
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
        public int HallId { get; set; }
        public Hall? Hall { get; set; }
        public List<Ticket>? Tickets { get; set; }
        public double Price { get; set; }
    }
}
