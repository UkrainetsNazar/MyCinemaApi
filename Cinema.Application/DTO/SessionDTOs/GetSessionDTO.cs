using Cinema.Application.DTO.HallDTOs;
using Cinema.Domain.Entities;

namespace Cinema.Application.DTO.SessionDTOs
{
    public class GetSessionDTO
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int HallId { get; set; }
        public DateTime StartTime { get; set; }
        public double Price { get; set; }
    }
}
