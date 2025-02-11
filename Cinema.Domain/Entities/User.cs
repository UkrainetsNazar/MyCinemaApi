using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.Entities
{
    public class User
    {
        int Id { get; set; }
        string? UserName { get; set; }
        string? Email { get; set; }
        string? Password { get; set; }
        List<Ticket>? Tickets { get; set; }
    }
}
