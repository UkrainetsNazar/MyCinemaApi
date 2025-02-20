using System.Text.Json.Serialization;

namespace Cinema.Application.DTO.TmdbDTO
{
    public class TmdbVideoResultDto
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("site")]
        public string? Site { get; set; }
    }
}
