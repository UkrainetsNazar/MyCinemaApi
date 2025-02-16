using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.DTO.TicketDTOs
{
    public class GetTicketForUserDTO
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public string? MovieTitle { get; set; }
        public DateTime SessionStartTime { get; set; }
        public int SeatNumber { get; set; }
        public int HallNumber { get; set; }
        public int RowNumber { get; set; }
    }
}
