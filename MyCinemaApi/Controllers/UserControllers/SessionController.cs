using Cinema.Application.UseCases;
using Cinema.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cinema.Presentation.Controllers.UserControllers
{
    [ApiController]
    [Route("api/session")]
    public class SessionController : ControllerBase
    {
        private readonly UseCaseManager _useCaseManager;
        private readonly ICacheService _cache;
        private readonly int DurationTime = 2;

        public SessionController(UseCaseManager useCaseManager, ICacheService redisCacheService)
        {
            _useCaseManager = useCaseManager;
            _cache = redisCacheService;
        }

        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetSession(int sessionId)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var cacheKey = $"session_{sessionId}";
                var cachedSession = _cache.Data<object>(cacheKey);

                if (cachedSession != null)
                {
                    stopwatch.Stop();
                    return Ok(ResponseCreator.Success(cachedSession, 200, stopwatch.Elapsed.TotalMilliseconds));
                }

                var session = await _useCaseManager.GetSessionDetailsHandler.HandleAsync(sessionId);
                _cache.SetData(cacheKey, session, DurationTime);

                stopwatch.Stop();
                return Ok(ResponseCreator.Success(session, 200, stopwatch.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return BadRequest(ResponseCreator.Error<object>(ex.Message, 400, stopwatch.Elapsed.TotalMilliseconds));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSessions()
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                const string cacheKey = "all_sessions";
                var cachedSessions = _cache.Data<object>(cacheKey);

                if (cachedSessions != null)
                {
                    stopwatch.Stop();
                    return Ok(ResponseCreator.Success(cachedSessions, 200, stopwatch.Elapsed.TotalMilliseconds));
                }

                var sessions = await _useCaseManager.GetAllSessionsHandler.HandleAsync();
                _cache.SetData(cacheKey, sessions, DurationTime);

                stopwatch.Stop();
                return Ok(ResponseCreator.Success(sessions, 200, stopwatch.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return BadRequest(ResponseCreator.Error<object>(ex.Message, 400, stopwatch.Elapsed.TotalMilliseconds));
            }
        }
    }
}
