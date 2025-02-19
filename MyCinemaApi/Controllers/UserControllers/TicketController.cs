using Cinema.Application.DTO.TicketDTOs;
using Cinema.Application.UseCases;
using Cinema.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cinema.Presentation.Controllers.UserControllers
{
    [ApiController]
    [Route("api/ticket")]
    public class TicketController : ControllerBase
    {
        private readonly UseCaseManager _useCaseManager;
        private readonly IRedisCacheService _cache;
        private readonly int DurationTime = 2;

        public TicketController(UseCaseManager useCaseManager, IRedisCacheService redisCacheService)
        {
            _useCaseManager = useCaseManager;
            _cache = redisCacheService;
        }

        [HttpGet("my-tickets")]
        public async Task<IActionResult> GetUserTickets(int UserId)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var cacheKey = $"user_tickets_{UserId}";
                var cachedTickets = _cache.Data<object>(cacheKey);

                if (cachedTickets != null)
                {
                    stopwatch.Stop();
                    return Ok(ResponseCreator.Success(cachedTickets, 200, stopwatch.Elapsed.TotalMilliseconds));
                }

                var ticket = await _useCaseManager.GetUserTicketsHandler.HandleAsync(UserId);
                _cache.SetData(cacheKey, ticket, DurationTime);

                stopwatch.Stop();
                return Ok(ResponseCreator.Success(ticket, 200, stopwatch.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return BadRequest(ResponseCreator.Error<object>(ex.Message, 400, stopwatch.Elapsed.TotalMilliseconds));
            }
        }

        [HttpPost("buy-ticket")]
        public async Task<IActionResult> BuyTicket([FromBody] CreateTicketDTO request)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var ticket = await _useCaseManager.BuyTicketHandler.HandleAsync(request.SessionId, request.SeatId, request.UserId);

                var cacheKey = $"user_tickets_{request.UserId}";
                _cache.SetData(cacheKey, ticket, DurationTime);

                stopwatch.Stop();
                return Ok(ResponseCreator.Success(ticket, 200, stopwatch.Elapsed.TotalMilliseconds));
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return BadRequest(ResponseCreator.Error<object>(ex.Message, 400, stopwatch.Elapsed.TotalMilliseconds));
            }
        }
    }
}
