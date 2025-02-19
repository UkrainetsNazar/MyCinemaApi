using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.Entities
{
    public class Hall
    {
        public int Id { get; set; }
        public int NumberOfHall { get; set; }
        public List<Session>? Sessions { get; set; } = new();
        public List<Row> Rows { get; set; } = new();
    }
}
