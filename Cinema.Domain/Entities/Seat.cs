namespace Cinema.Domain.Entities
{
    public class Seat
    {
        public int Id { get; set; }
        public int RowId { get; set; }
        public Row? Row { get; set; }
        public int SeatNumber { get; set; }
        public bool IsBooked { get; set; }
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }
    }
}
