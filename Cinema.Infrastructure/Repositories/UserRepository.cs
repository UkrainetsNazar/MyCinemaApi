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
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(CinemaDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                _logger.LogInformation("Adding user with username {UserName} and email {Email}", user.UserName, user.Email);
                await _context.Users.AddAsync(user);
                _logger.LogInformation("User {UserName} added successfully", user.UserName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user with username {UserName}", user.UserName);
                throw;
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting user with id {UserId}", id);
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    _logger.LogInformation("User with id {UserId} deleted successfully", id);
                }
                else
                {
                    _logger.LogWarning("User with id {UserId} not found", id);
                    throw new Exception("User not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with id {UserId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all users");
                var users = await _context.Users.ToListAsync();
                _logger.LogInformation("Fetched {UserCount} users", users.Count);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users");
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching user with id {UserId}", id);
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _logger.LogInformation("User with id {UserId} found", id);
                    return user;
                }
                else
                {
                    _logger.LogWarning("User with id {UserId} not found", id);
                    throw new Exception($"User with id {id} not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user with id {UserId}", id);
                throw;
            }
        }

        public async Task UpdateUserAsync(int id, User user)
        {
            try
            {
                _logger.LogInformation("Updating user with id {UserId}", id);
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (existingUser != null)
                {
                    existingUser.UserName = user.UserName;
                    existingUser.Email = user.Email;
                    existingUser.Password = user.Password;
                    _logger.LogInformation("User with id {UserId} updated successfully", id);
                }
                else
                {
                    _logger.LogWarning("User with id {UserId} not found", id);
                    throw new Exception("User not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with id {UserId}", id);
                throw;
            }
        }
    }
}
