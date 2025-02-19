using Cinema.Application.DTO.MovieDTOs;
using Cinema.Application.UseCases;
using Cinema.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cinema.Presentation.Controllers.UserControllers
{
    [ApiController]
    [Route("api/movie")]
    public class MovieController : ControllerBase
    {
        private readonly UseCaseManager _useCaseManager;
        private readonly IRedisCacheService _cache;
        private readonly int DurationTime = 10;

        public MovieController(UseCaseManager useCaseManager, IRedisCacheService redisCacheService)
        {
            _useCaseManager = useCaseManager;
            _cache = redisCacheService;
        }

        [HttpGet("{id}/sessions")]
        public async Task<IActionResult> GetMovieWithSessions(int id, [FromQuery] DateTime date)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var cacheKey = $"movie_{id}_sessions_{date:yyyyMMdd}";
                var cachedData = _cache.Data<object>(cacheKey);

                if (cachedData != null)
                {
                    stopwatch.Stop();
                    return Ok(ResponseCreator.Success(cachedData, 200, stopwatch.Elapsed.TotalMilliseconds));
                }

                var movieWithSessions = await _useCaseManager.GetMovieWithSessionsHandler.HandleAsync(id, date);
                _cache.SetData(cacheKey, movieWithSessions, DurationTime);

                stopwatch.Stop();
                return Ok(ResponseCreator.Success(movieWithSessions, 200, stopwatch.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return BadRequest(ResponseCreator.Error<object>(ex.Message, 400, stopwatch.Elapsed.TotalMilliseconds));
            }
        }

        [HttpGet("movies")]
        public async Task<IActionResult> GetMoviesByDate([FromQuery] DateTime date)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var cacheKey = $"movies_{date:yyyyMMdd}";
                var cachedData = _cache.Data<object>(cacheKey);

                if (cachedData != null)
                {
                    stopwatch.Stop();
                    return Ok(ResponseCreator.Success(cachedData, 200, stopwatch.Elapsed.TotalMilliseconds));
                }

                var movies = await _useCaseManager.GetMoviesByDateHandler.HandleAsync(date);
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

        [HttpPost("rate-movie")]
        public async Task<IActionResult> RateMovie([FromBody] MovieRatingDTO request)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var updatedMovie = await _useCaseManager.MovieRatingHandler.HandleAsync(request.MovieId, request.Rating);

                var cachePattern = $"movie_{request.MovieId}_sessions_";
                _cache.ClearDataByPatternAsync(cachePattern);

                stopwatch.Stop();
                return Ok(ResponseCreator.Success(updatedMovie, 200, stopwatch.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return BadRequest(ResponseCreator.Error<object>(ex.Message, 400, stopwatch.Elapsed.TotalMilliseconds));
            }
        }
    }
}
