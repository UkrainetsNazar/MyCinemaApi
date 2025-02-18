using Cinema.Domain.Entities;

namespace Cinema.Application.Interfaces
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetAllSeatsAsync();
        Task<Seat> GetSeatByIdAsync(int id);
        Task AddSeatAsync(Seat seat);
        Task UpdateSeatAsync(int id, Seat seat);
        Task DeleteSeatAsync(int id);
        Task<IEnumerable<Seat>> GetSeatsByHallAsync(int hallId);

    }
}
