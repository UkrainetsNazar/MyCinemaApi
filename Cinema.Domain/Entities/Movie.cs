using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Cinema.Domain.Entities
{
    public class Movie
    {
        int Id { get; set; }
        string? MovieTitle { get; set; }
        string? Description { get; set; }
        int DurationMinutes { get; set; }
        string? Genre { get; set; }
        string? PosterUrl { get; set; }
        string? TrailerUrl { get; set; }
        DateTime ReleaseDate { get; set; }
        double Rating { get; set; }
        List<Session>? Sessions { get; set; }
    }
}
