using Cinema.Application.Interfaces;
using Cinema.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cinema.Presentation.Controllers.AdminControllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/tmdb")]
    [ApiController]
    public class AdminMovieController : ControllerBase
    {
        private readonly TmdbService _tmdbService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        private readonly int DurationTime = 10;

        public AdminMovieController(TmdbService tmdbService, ICacheService redisCacheService, IUnitOfWork unitOfWork)
        {
            _tmdbService = tmdbService;
            _cache = redisCacheService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMovie([FromQuery] int tmdbId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var stopwatch = Stopwatch.StartNew();
            var user = User;
            try
            {
                var cacheKey = $"movie_{tmdbId}";

                var cachedData = _cache.Data<object>(cacheKey);
                if (cachedData != null)
                {
                    stopwatch.Stop();
                    return Ok(ResponseCreator.Success(cachedData, 200, stopwatch.Elapsed.TotalMilliseconds));
                }

                var movie = await _tmdbService.AddMovieFromTmdbAsync(tmdbId, startDate, endDate);

                if (movie == null)
                {
                    stopwatch.Stop();
                    return BadRequest(ResponseCreator.Error<object>("Не вдалося додати фільм або він уже є в базі.", 400, stopwatch.Elapsed.TotalMilliseconds));
                }

                _cache.SetData(cacheKey, movie, DurationTime);

                stopwatch.Stop();
                return Ok(ResponseCreator.Success(movie, 200, stopwatch.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return BadRequest(ResponseCreator.Error<object>(ex.Message, 400, stopwatch.Elapsed.TotalMilliseconds));
            }
        }

        [HttpGet("get_all")]
        public async Task<IActionResult> GetAllMovies()
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var cacheKey = "movies";
                var cachedData = _cache.Data<object>(cacheKey);

                if (cachedData != null)
                {
                    stopwatch.Stop();
                    return Ok(ResponseCreator.Success(cachedData, 200, stopwatch.Elapsed.TotalMilliseconds));
                }

                var movies = await _unitOfWork.Movies.GetAllMoviesAsync();
                _cache.SetData(cacheKey, movies, DurationTime);
                stopwatch.Stop();

                return Ok(ResponseCreator.Success(movies, 200, stopwatch.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return BadRequest(ResponseCreator.Error<object>(ex.Message, 400, stopwatch.Elapsed.TotalMilliseconds));
            }
        }
    }
}
