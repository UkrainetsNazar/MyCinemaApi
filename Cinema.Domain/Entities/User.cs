using Microsoft.AspNetCore.Identity;

namespace Cinema.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Role { get; set; } = string.Empty;
        public List<Ticket> Tickets { get; set; } = new();
    }
}
