using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.DTO.SeatDTOs
{
    public class CreateSeatDTO
    {
        public int RowId { get; set; }
        public int SeatNumber { get; set; }
        public bool IsBooked { get; set; }
    }
}
