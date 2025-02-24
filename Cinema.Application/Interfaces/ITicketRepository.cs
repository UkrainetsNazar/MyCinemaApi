using Cinema.Domain.Entities;

namespace Cinema.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<Ticket> GetTicketByIdAsync(int id);
        Task AddTicketAsync(Ticket ticket);
        Task UpdateTicketAsync(int id, Ticket ticket);
        Task DeleteTicketAsync(int id);
        Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(string userId);
    }
}
