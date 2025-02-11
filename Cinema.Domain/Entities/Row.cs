using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.Entities
{
    public class Row
    {
        int Id { get; set; }
        int RowNumber { get; set; }
        int SeatCount { get; set; }
        int HallId { get; set; }
        Hall? Hall { get; set; }
        List<Seat>? Seats { get; set; }
    }
}
