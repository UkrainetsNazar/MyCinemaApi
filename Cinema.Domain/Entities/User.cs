using Microsoft.AspNetCore.Identity;

namespace Cinema.Domain.Entities
{
    public class User : IdentityUser
    {
        public List<Ticket> Tickets { get; set; } = new();
    }
}
