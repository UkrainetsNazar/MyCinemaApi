using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;
using Cinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly CinemaDbContext _context;
        public TicketRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
        }

        public async Task DeleteTicketAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
            else
            {
                throw new Exception("Ticket not found");
            }
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                return ticket;
            }
            else
            {
                throw new Exception($"Ticket with id {id} not found");
            }
        }

        public async Task UpdateTicketAsync(int id, Ticket ticket)
        {
            var existingTicket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (existingTicket != null)
            {
                existingTicket.SessionId = ticket.SessionId;
                existingTicket.SeatId = ticket.SeatId;
                existingTicket.UserId = ticket.UserId;
            }
            else
            {
                throw new Exception("Ticket not found");
            }
        }
        public async Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(int userId)
        {
            return await _context.Tickets
                .Include(t => t.Session)
                .ThenInclude(s => s.Movie)
                .Include(t => t.Seat)
                .ThenInclude(s => s.Row)
                .Include(t => t.Session!.Hall)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }
    }
}
