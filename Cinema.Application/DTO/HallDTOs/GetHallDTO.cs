using Cinema.Application.DTO.RowDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.DTO.HallDTOs
{
    public class GetHallDTO
    {
        public int Id { get; set; }
        public int NumberOfHall { get; set; }
        public List<GetRowDTO>? Rows { get; set; }
    }
}
