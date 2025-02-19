using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema.Domain.Entities
{
    public class Row
    {
        public int Id { get; set; }
        public int RowNumber { get; set; }
        [NotMapped]
        public int SeatCount => Seats.Count;
        public int HallId { get; set; }
        public Hall? Hall { get; set; }
        public List<Seat> Seats { get; set; } = new();
    }
}
