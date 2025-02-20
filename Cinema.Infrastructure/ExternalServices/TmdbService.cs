using AutoMapper;
using Cinema.Application.DTO.TmdbDTO;
using Cinema.Application.Interfaces;
using Cinema.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Cinema.Infrastructure.ExternalServices
{
    public class TmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TmdbService(IMapper mapper, IOptions<TmdbSettings> tmdbSettings, IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork)
        {
            _apiKey = tmdbSettings.Value.ApiKey!;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://api.themoviedb.org/3/");
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string?> GetMovieTrailerAsync(int movieId)
        {
            var response = await _httpClient.GetAsync($"movie/{movieId}/videos?api_key={_apiKey}");

            if (!response.IsSuccessStatusCode)
                return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var videoData = JsonSerializer.Deserialize<TmdbVideoDto>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var trailer = videoData?.Results?.FirstOrDefault(v => v.Type == "Trailer" && v.Site == "YouTube");
            return trailer != null ? $"https://www.youtube.com/watch?v={trailer.Key}" : null;
        }


        public async Task<Movie?> AddMovieFromTmdbAsync(int tmdbId, DateTime startDate, DateTime endDate, string language = "en-UA")
        {
            Console.WriteLine($"Fetching movie with ID {tmdbId} from TMDB...");

            if (await _unitOfWork.Movies.MovieExistsAsync(tmdbId))
            {
                Console.WriteLine("Movie already exists in the database.");
                return null;
            }

            var response = await _httpClient.GetAsync($"movie/{tmdbId}?api_key={_apiKey}&language={language}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error fetching movie: {errorMessage}");
                return null;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var tmdbMovie = JsonSerializer.Deserialize<TmdbDto>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (tmdbMovie == null)
                return null;

            var trailerUrl = await GetMovieTrailerAsync(tmdbId);

            var movieEntity = _mapper.Map<Movie>(tmdbMovie);

            movieEntity.Id = tmdbId;
            movieEntity.TrailerUrl = trailerUrl;
            movieEntity.StartDate = startDate;
            movieEntity.EndDate = endDate;
            movieEntity.Rating = 0;
            movieEntity.RatingCount = 0;
            movieEntity.Sessions = new List<Session>();

            await _unitOfWork.Movies.AddMovieAsync(movieEntity);
            await _unitOfWork.SaveChangesAsync();
            return movieEntity;
        }

    }
}
