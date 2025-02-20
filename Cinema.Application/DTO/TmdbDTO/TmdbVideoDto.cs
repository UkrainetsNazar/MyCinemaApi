using System.Text.Json.Serialization;

namespace Cinema.Application.DTO.TmdbDTO
{
    public class TmdbVideoDto
    {
        [JsonPropertyName("results")]
        public List<TmdbVideoResultDto>? Results { get; set; }
    }
}
