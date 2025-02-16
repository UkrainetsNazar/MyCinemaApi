using Cinema.Domain.Entities;
using Cinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Cinema.Application.Interfaces;

namespace Cinema.Infrastructure.Repositories
{
    public class HallRepository : IHallRepository
    {
        private readonly CinemaDbContext _context;
        public HallRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task AddHallAsync(Hall hall)
        {
            await _context.Halls.AddAsync(hall);
        }

        public async Task DeleteHallAsync(int id)
        {
            var hall = await _context.Halls.FindAsync(id);
            if (hall != null)
            {
                _context.Halls.Remove(hall);
            }
            else
            {
                throw new Exception("Hall not found");
            }
        }

        public async Task<IEnumerable<Hall>> GetAllHallsAsync()
        {
            return await _context.Halls.ToListAsync();
        }

        public async Task<Hall> GetHallByIdAsync(int id)
        {
            var hall = await _context.Halls.FindAsync(id);
            if (hall != null)
            {
                return hall;
            }
            else
            {
                throw new Exception($"Hall with id {id} not found");
            }
        }

        public async Task UpdateHallAsync(int id, Hall hall)
        {
            var existingHall = await _context.Halls.FirstOrDefaultAsync(h => h.Id == id);
            if (existingHall != null)
            {
                existingHall.NumberOfHall = hall.NumberOfHall;
                existingHall.Sessions = hall.Sessions;
                existingHall.Rows = hall.Rows;
            }
            else
            {
                throw new Exception("Hall not found");
            }
        }
        public async Task<Hall?> GetByNumberAsync(int hallNumber)
        {
            return await _context.Halls
                .Include(h => h.Rows!)
                .ThenInclude(r => r.Seats)
                .FirstOrDefaultAsync(h => h.NumberOfHall == hallNumber);
        }

    }
}
