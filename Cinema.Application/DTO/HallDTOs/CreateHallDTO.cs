using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.DTO.HallDTOs
{
    public class CreateHallDTO
    {
        public int HallNumber { get; set; }
        public int RowCount { get; set; }
        public int SeatsPerRow { get; set; }
    }
}
