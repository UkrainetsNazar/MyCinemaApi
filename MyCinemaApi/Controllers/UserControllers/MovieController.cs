using Cinema.Application.DTO.MovieDTOs;
using Cinema.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cinema.Presentation.Controllers.UserControllers
{
    [ApiController]
    [Route("api/movie")]
    public class MovieController : ControllerBase
    {
        private readonly UseCaseManager _useCaseManager;

        public MovieController(UseCaseManager useCaseManager)
        {
            _useCaseManager = useCaseManager;
        }

        [HttpGet("{id}/sessions")]
        public async Task<IActionResult> GetMovieWithSessions(int id, [FromQuery] DateTime date)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var movieWithSessions = await _useCaseManager.GetMovieWithSessionsHandler.HandleAsync(id, date);
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
                var movies = await _useCaseManager.GetMoviesByDateHandler.HandleAsync(date);
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
