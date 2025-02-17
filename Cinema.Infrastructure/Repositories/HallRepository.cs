using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;
using Cinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cinema.Infrastructure.Repositories
{
    public class HallRepository : IHallRepository
    {
        private readonly CinemaDbContext _context;
        private readonly ILogger<HallRepository> _logger;

        public HallRepository(CinemaDbContext context, ILogger<HallRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddHallAsync(Hall hall)
        {
            try
            {
                _logger.LogInformation("Adding hall: {HallNumber}", hall.NumberOfHall);
                await _context.Halls.AddAsync(hall);
                _logger.LogInformation("Hall added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding hall: {HallNumber}", hall.NumberOfHall);
                throw;
            }
        }

        public async Task DeleteHallAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting hall with id {HallId}", id);
                var hall = await _context.Halls.FindAsync(id);
                if (hall != null)
                {
                    _context.Halls.Remove(hall);
                    _logger.LogInformation("Hall with id {HallId} deleted successfully", id);
                }
                else
                {
                    _logger.LogWarning("Hall with id {HallId} not found", id);
                    throw new Exception("Hall not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting hall with id {HallId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Hall>> GetAllHallsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all halls");
                var halls = await _context.Halls.ToListAsync();
                _logger.LogInformation("Fetched {HallCount} halls", halls.Count);
                return halls;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all halls");
                throw;
            }
        }

        public async Task<Hall> GetHallByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching hall with id {HallId}", id);
                var hall = await _context.Halls.FindAsync(id);
                if (hall != null)
                {
                    _logger.LogInformation("Hall with id {HallId} found", id);
                    return hall;
                }
                else
                {
                    _logger.LogWarning("Hall with id {HallId} not found", id);
                    throw new Exception($"Hall with id {id} not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching hall with id {HallId}", id);
                throw;
            }
        }

        public async Task UpdateHallAsync(int id, Hall hall)
        {
            try
            {
                _logger.LogInformation("Updating hall with id {HallId}", id);
                var existingHall = await _context.Halls.FirstOrDefaultAsync(h => h.Id == id);
                if (existingHall != null)
                {
                    existingHall.NumberOfHall = hall.NumberOfHall;
                    existingHall.Sessions = hall.Sessions;
                    existingHall.Rows = hall.Rows;

                    _logger.LogInformation("Hall with id {HallId} updated successfully", id);
                }
                else
                {
                    _logger.LogWarning("Hall with id {HallId} not found", id);
                    throw new Exception("Hall not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating hall with id {HallId}", id);
                throw;
            }
        }

        public async Task<Hall?> GetByNumberAsync(int hallNumber)
        {
            try
            {
                _logger.LogInformation("Fetching hall with number {HallNumber}", hallNumber);
                var hall = await _context.Halls
                    .Include(h => h.Rows!)
                    .ThenInclude(r => r.Seats)
                    .FirstOrDefaultAsync(h => h.NumberOfHall == hallNumber);

                if (hall != null)
                {
                    _logger.LogInformation("Hall with number {HallNumber} found", hallNumber);
                }
                else
                {
                    _logger.LogWarning("Hall with number {HallNumber} not found", hallNumber);
                }

                return hall;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching hall with number {HallNumber}", hallNumber);
                throw;
            }
        }
    }
}
