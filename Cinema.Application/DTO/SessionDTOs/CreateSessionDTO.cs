using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.DTO.SessionDTOs
{
    public class CreateSessionDTO
    {
        public int MovieId { get; set; }
        public DateTime StartTime { get; set; }
        public double Price { get; set; }
        public int NumberOfHall { get; set; }
    }
}
