using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;
using Cinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cinema.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly CinemaDbContext _context;
        private readonly ILogger<TicketRepository> _logger;
        public TicketRepository(CinemaDbContext context, ILogger<TicketRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            try
            {
                _logger.LogInformation("Adding ticket with sessionId {SessionId}, seatId {SeatId}, userId {UserId}", ticket.SessionId, ticket.SeatId, ticket.UserId);
                await _context.Tickets.AddAsync(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ticket with sessionId {SessionId}", ticket.SessionId);
                throw;
            }
        }

        public async Task DeleteTicketAsync(int id)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(id);
                if (ticket != null)
                {
                    _context.Tickets.Remove(ticket);
                    _logger.LogInformation("Ticket with id {TicketId} deleted successfully", id);
                }
                else
                {
                    _logger.LogWarning("Ticket with id {TicketId} not found", id);
                    throw new Exception("Ticket not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ticket with id {TicketId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all tickets");
                var tickets = await _context.Tickets.ToListAsync();
                _logger.LogInformation("Fetched {TicketCount} tickets", tickets.Count);
                return tickets;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all tickets");
                throw;
            }
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching ticket with id {TicketId}", id);
                var ticket = await _context.Tickets.FindAsync(id);
                if (ticket != null)
                {
                    _logger.LogInformation("Ticket with id {TicketId} found", id);
                    return ticket;
                }
                else
                {
                    _logger.LogWarning("Ticket with id {TicketId} not found", id);
                    throw new Exception($"Ticket with id {id} not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ticket with id {TicketId}", id);
                throw;
            }
        }

        public async Task UpdateTicketAsync(int id, Ticket ticket)
        {
            try
            {
                _logger.LogInformation("Updating ticket with id {TicketId}", id);
                var existingTicket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
                if (existingTicket != null)
                {
                    existingTicket.SessionId = ticket.SessionId;
                    existingTicket.SeatId = ticket.SeatId;
                    existingTicket.UserId = ticket.UserId;
                    _logger.LogInformation("Ticket with id {TicketId} updated successfully", id);
                }
                else
                {
                    _logger.LogWarning("Ticket with id {TicketId} not found", id);
                    throw new Exception("Ticket not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ticket with id {TicketId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching tickets for userId {UserId}", userId);
                var tickets = await _context.Tickets
                    .Include(t => t.Session)
                    .ThenInclude(s => s!.Movie)
                    .Include(t => t.Seat)
                    .ThenInclude(s => s!.Row)
                    .Include(t => t.Session!.Hall)
                    .Where(t => t.UserId == userId)
                    .ToListAsync();
                _logger.LogInformation("Fetched {TicketCount} tickets for userId {UserId}", tickets.Count, userId);
                return tickets;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tickets for userId {UserId}", userId);
                throw;
            }
        }
    }
}
