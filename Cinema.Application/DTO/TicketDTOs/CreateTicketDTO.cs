using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.DTO.TicketDTOs
{
    public class CreateTicketDTO
    {
        public int SessionId { get; set; }
        public string? UserId { get; set; }
        public int SeatId { get; set; }
    }
}
