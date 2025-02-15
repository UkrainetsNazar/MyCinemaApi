using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.Entities
{
    public class Row
    {
        public int Id { get; set; }
        public int RowNumber { get; set; }
        public int SeatCount { get; set; }
        public int HallId { get; set; }
        public Hall? Hall { get; set; }
        public List<Seat>? Seats { get; set; }
    }
}
