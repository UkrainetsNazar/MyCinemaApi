using Cinema.Application.DTO.SeatDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.DTO.RowDTOs
{
    public class GetRowDTO
    {
        public int Id { get; set; }
        public int RowNumber { get; set; }
        public int SeatCount { get; set; }
        public List<GetSeatDTO>? Seats { get; set; }
    }
}
