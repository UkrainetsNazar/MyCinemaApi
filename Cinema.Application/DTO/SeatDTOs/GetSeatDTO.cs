using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.DTO.SeatDTOs
{
    public class GetSeatDTO
    {
        public int? Id { get; set; }
        public int? RowNumber { get; set; }
        public int? SeatNumber { get; set; }
        public bool IsBooked { get; set; }
    }
}
