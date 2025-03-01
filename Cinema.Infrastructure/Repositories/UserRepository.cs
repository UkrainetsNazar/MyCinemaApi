using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;
using Cinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cinema.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CinemaDbContext _context;
        private readonly ILogger<HallRepository> _logger;

        public UserRepository(CinemaDbContext context, ILogger<HallRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddUser(User user)
        {
            try
            {
                _logger.LogInformation("Adding user: {Email}", user.Email);
                await _context.Users.AddAsync(user);
                _logger.LogInformation("User added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user: {Email}", user.Email);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Getting all users");
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                throw;
            }
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                _logger.LogInformation("Getting user by email: {Email}", email);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email: {Email}", email);
                throw;
            }
        }
    }
}
