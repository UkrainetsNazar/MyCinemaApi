using Cinema.Application.DTO.SessionDTOs;
using Cinema.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cinema.Presentation.Controllers.AdminControllers
{
    [ApiController]
    [Route("api/admin/session")]
    public class AdminSessionController : ControllerBase
    {
        private readonly UseCaseManager _useCaseManager;

        public AdminSessionController(UseCaseManager useCaseManager)
        {
            _useCaseManager = useCaseManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionDTO addSessionDTO)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _useCaseManager.AddSessionHandler.HandleAsync(addSessionDTO);
                stopwatch.Stop();
                return Ok(ResponseCreator.Success("Сеанс успішно додано", 200, stopwatch.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return BadRequest(ResponseCreator.Error<object>(ex.Message, 400, stopwatch.Elapsed.TotalMilliseconds));
            }
        }

        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetSession(int sessionId)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var session = await _useCaseManager.GetSessionDetailsHandler.HandleAsync(sessionId);
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
                var sessions = await _useCaseManager.GetAllSessionsHandler.HandleAsync();
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
