using Cinema.Domain.Entities;

namespace Cinema.Application.Interfaces
{
    public interface IHallRepository
    {
        Task<IEnumerable<Hall>> GetAllHallsAsync();
        Task<Hall> GetHallByIdAsync(int id);
        Task AddHallAsync(Hall hall);
        Task UpdateHallAsync(int id, Hall hall);
        Task DeleteHallAsync(int id);
        Task<Hall?> GetByNumberAsync(int hallNumber);
    }
}
