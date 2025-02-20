using System.Text.Json.Serialization;

namespace Cinema.Application.DTO.TmdbDTO
{
    public class TmdbDto
    {
        [JsonPropertyName("title")]
        public string? MovieTitle { get; set; }

        [JsonPropertyName("overview")]
        public string? Description { get; set; }

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }
    }
}
