using Cinema.Application.DTO.TicketDTOs;
using Cinema.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cinema.Presentation.Controllers.UserControllers
{
    [ApiController]
    [Route("api/ticket")]
    public class TicketController : ControllerBase
    {
        private readonly UseCaseManager _useCaseManager;

        public TicketController(UseCaseManager useCaseManager)
        {
            _useCaseManager = useCaseManager;
        }

        [HttpGet("my-tickets")]
        public async Task<IActionResult> GetUserTickets(int UserId) //Add JWT token (var userId = User.FindFirst("sub")?.Value;)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var ticket = await _useCaseManager.GetUserTicketsHandler.HandleAsync(UserId);
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
