using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.DTO.MovieDTOs
{
    public class UpdateMovieDTO
    {
        public string? MovieTitle { get; set; }
        public string? Description { get; set; }
        public int? DurationMinutes { get; set; }
        public string? Genre { get; set; }
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public double Rating { get; set; }
    }
}
