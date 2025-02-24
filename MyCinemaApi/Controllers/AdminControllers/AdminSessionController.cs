using Cinema.Application.DTO.SessionDTOs;
using Cinema.Application.UseCases;
using Cinema.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cinema.Presentation.Controllers.AdminControllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/session")]
    public class AdminSessionController : ControllerBase
    {
        private readonly UseCaseManager _useCaseManager;
        private readonly ICacheService _cache;

        public AdminSessionController(UseCaseManager useCaseManager, ICacheService redisCacheService)
        {
            _useCaseManager = useCaseManager;
            _cache = redisCacheService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionDTO addSessionDTO)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _useCaseManager.AddSessionHandler.HandleAsync(addSessionDTO);

                var cachePattern = $"movie_{addSessionDTO.MovieId}_sessions_";
                _cache.ClearDataByPattern(cachePattern);

                stopwatch.Stop();
                return Ok(ResponseCreator.Success("Сеанс успішно додано", 200, stopwatch.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return BadRequest(ResponseCreator.Error<object>(ex.Message, 400, stopwatch.Elapsed.TotalMilliseconds));
            }
        }
    }
}
