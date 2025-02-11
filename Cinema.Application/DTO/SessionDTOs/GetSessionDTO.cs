using Cinema.Application.DTO.HallDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.DTO.SessionDTOs
{
    public class GetSessionDTO
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public double Price { get; set; }

        public int MovieId { get; set; }
        public string? MovieTitle { get; set; }
        public string? PosterUrl { get; set; }

        public GetHallDTO? Hall { get; set; }
    }
}
