using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.Entities
{
    public class Hall
    {
        int Id { get; set; }
        int NumberOfHall { get; set; }
        List<Session>? Sessions { get; set; }
        List<Row>? Rows { get; set; }
    }
}
