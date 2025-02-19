namespace Cinema.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string? MovieTitle { get; set; }
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public string? Genre { get; set; }
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RatingCount { get; set; }
        public double Rating { get; set; }
        public List<Session>? Sessions { get; set; }
    }
}
