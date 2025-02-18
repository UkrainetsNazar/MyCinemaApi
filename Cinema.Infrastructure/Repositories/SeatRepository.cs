using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;
using Cinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cinema.Infrastructure.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly CinemaDbContext _context;
        private readonly ILogger<HallRepository> _logger;

        public SeatRepository(CinemaDbContext context, ILogger<HallRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddSeatAsync(Seat seat)
        {
            try
            {
                _logger.LogInformation("Adding seat: {SeatNumber}", seat.SeatNumber);
                await _context.Seats.AddAsync(seat);
                _logger.LogInformation("Seat added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding seat: {SeatNumber}", seat.SeatNumber);
                throw;
            }
        }

        public async Task DeleteSeatAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting seat with id {SeatId}", id);
                var seat = await _context.Seats.FindAsync(id);
                if (seat != null)
                {
                    _context.Seats.Remove(seat);
                    _logger.LogInformation("Seat with id {SeatId} deleted successfully", id);
                }
                else
                {
                    _logger.LogWarning("Seat with id {SeatId} not found", id);
                    throw new Exception("Seat not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting seat with id {SeatId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Seat>> GetAllSeatsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all seats");
                return await _context.Seats.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all seats");
                throw;
            }
        }

        public async Task<Seat> GetSeatByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Seat id is <= 0");
                    throw new Exception("Seat id is <= 0");
                }
                _logger.LogInformation("Fetching seat with id {SeatId}", id);
                return await _context.Seats.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching seat with id {SeatId}", id);
                throw;
            }
        }

        public async Task UpdateSeatAsync(int Id, Seat seat)
        {
            try
            {
                var existingSeat = await _context.Seats.FindAsync(Id);
                _logger.LogInformation("Updating seat with id {SeatId}", seat.Id);
                _context.Seats.Update(seat);
                _logger.LogInformation("Seat with id {SeatId} updated successfully", seat.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating seat with id {SeatId}", seat.Id);
                throw;
            }
        }

        public async Task<IEnumerable<Seat>> GetSeatsByHallAsync(int hallId)
        {
            try
            {
                _logger.LogInformation("Fetching all seats by hall id {HallId}", hallId);
                return await _context.Seats.Where(s => s.Row!.HallId == hallId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all seats by hall id {HallId}", hallId);
                throw;
            }
        }
    }
}
